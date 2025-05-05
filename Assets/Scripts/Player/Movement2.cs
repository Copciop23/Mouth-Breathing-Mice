using System;
using System.Collections;
using UnityEngine;

public class MovementP2 : MonoBehaviour
{
    private float horizontal;
    public float speed = 6f;
    public float jumpingPower = 8f;
    private bool isFacingRight = true;
    private bool canJump = true;
    private bool recentlyLanded = false;
    private bool isPunching = false;
    private bool canDoubleJump = false;
    private bool ChargingJump = false;
    private bool isBlocking = false;
    private bool isCrouching = false;
    private const float immobilityTolerance = 0.01f;
    private const float immobilityThreshold = 0.2f;
    private Vector3 lastPosition;
    private float immobilityTime;

    // Player 2 Controls
    private KeyCode jumpKey = KeyCode.UpArrow;
    private KeyCode blockKey = KeyCode.Keypad0;
    private KeyCode crouchKey = KeyCode.DownArrow;
    private KeyCode punchKey = KeyCode.Keypad1;
    private KeyCode dashKey = KeyCode.Keypad2;
    private KeyCode fireballKey = KeyCode.Keypad3;
    private KeyCode hawkTuahKey = KeyCode.Keypad4;
    private KeyCode shieldKey = KeyCode.Keypad5;
    private KeyCode chargeJumpKey = KeyCode.RightControl;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform playerSprite;
    [SerializeField] private Animator animator;
    private SpringBootsP2 springboots;
    private PlayerStats playerStats;

    [SerializeField] private ShieldController shieldController;
    private float blockDuration = 0.6f; // How long blocking lasts
    private float blockCooldown = 4.5f; // Cooldown between blocks
    private float lastBlockTime = -10f; // Initialize to allow immediate first block

    public bool IsFacingRight => isFacingRight;

    private void Start()
    {
        if (shieldController == null) shieldController = GetComponent<ShieldController>();

        playerStats = GetComponent<PlayerStats>();
        if (springboots == null)
        {
            springboots = GetComponent<SpringBootsP2>();
        }
    }

    void Update()
    {
        // Manual input with Arrow keys
        horizontal = Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            StartBlocking();
            AudioManager.Instance.PlaySound("fireball", AudioManager.AudioType.SFX);
        }
        // Removed the StopBlocking on key up to match movement.cs
        // else if (Input.GetKeyUp(KeyCode.RightShift))
        // {
        //     StopBlocking();
        // }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCrouching();
            AudioManager.Instance.PlaySound("h", AudioManager.AudioType.SFX);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            StopCrouching();
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow)) && IsGrounded() && canJump && !recentlyLanded)
        {
            Jump(playerStats.Attributes.JumpPower);
            canDoubleJump = true;
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow)) && canDoubleJump && !IsGrounded())
        {
            Jump(jumpingPower * 0.8f);
            canDoubleJump = false;
        }

        if ((Input.GetKeyUp(KeyCode.UpArrow)) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.RightControl) && !isPunching)
        {
            StartCoroutine(PunchAction());
        }

        if (springboots.IsChargingJump)
        {
            StartCoroutine(ChargeJump());
        }

        DetectImmobility();

        if (IsGrounded() || !IsImmobile())
        {
            Flip();
            UpdateAnimation();
        }
        else
        {
            rb.position += new Vector2(0, -0.0002f);
            animator.CrossFade("inWall", 0, 0);
        }
    }

    private void FixedUpdate()
    {
        if (!isBlocking && !isCrouching)
        {
            rb.linearVelocity = new Vector2(horizontal * playerStats.Attributes.Speed, rb.linearVelocity.y);
        }
    }

    private void StartBlocking()
    {
        if (Time.time >= lastBlockTime + blockCooldown)
        {
            StartCoroutine(HandleBlocking());
        }
    }

    private IEnumerator HandleBlocking()
    {
        isBlocking = true;
        animator.CrossFade("block", 0, 0);

        // Blocking lasts for the duration
        yield return new WaitForSeconds(blockDuration);

        isBlocking = false;
        animator.CrossFade("idle", 0, 0);

        // Set cooldown
        lastBlockTime = Time.time;

        // Optional: Provide feedback that blocking is on cooldown
        Debug.Log("Blocking is on cooldown.");
    }

    private void StartCrouching()
    {
        isCrouching = true;
        animator.CrossFade("down", 0, 0);
    }

    private void StopCrouching()
    {
        isCrouching = false;
        animator.CrossFade("idle", 0, 0);
    }

    private void DetectImmobility()
    {
        float positionChange = Vector3.Distance(transform.position, lastPosition);

        if (positionChange < immobilityTolerance && rb.linearVelocity.magnitude < immobilityTolerance && !IsGrounded())
        {
            immobilityTime += Time.deltaTime;
        }
        else
        {
            immobilityTime = 0f;
        }

        lastPosition = transform.position;
    }

    private bool IsImmobile()
    {
        return immobilityTime >= immobilityThreshold;
    }

    private bool IsGrounded()
    {
        bool grounded = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayer);
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.000001f, groundLayer);
        if (hit.collider != null && hit.collider.CompareTag("Death"))
        {
            SendMessage("onblocktouched", hit.collider.gameObject, SendMessageOptions.DontRequireReceiver);
        }
        if (grounded)
        {
            canDoubleJump = false;
        }
        return grounded;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = playerSprite.localScale;
            localScale.x *= -1f;
            playerSprite.localScale = localScale;
        }
    }

    private void UpdateAnimation()
    {
        if (isPunching) return;
        if (ChargingJump) return;
        if (isBlocking) return;
        if (isCrouching) return;

        if (!IsGrounded() && rb.linearVelocity.y > 0)
        {
            animator.CrossFade("jump", 0, 0);
        }
        else if (rb.linearVelocity.y < 0)
        {
            animator.CrossFade("landing", 0, 0);
        }
        else if (horizontal != 0 && IsGrounded())
        {
            animator.CrossFade("running", 0, 0);
        }
        else
        {
            animator.CrossFade("idle", 0, 0);
        }
    }

    private void Jump(float power)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, power);
        AudioManager.Instance.PlaySound("jump", AudioManager.AudioType.SFX);
        canJump = false;
        StartCoroutine(JumpCooldown());
    }

    private IEnumerator PunchAction()
    {
        isPunching = true;
        animator.CrossFade("punch", 0, 0);

        yield return new WaitForSeconds(0.30f);

        isPunching = false;
    }

    private IEnumerator ChargeJump()
    {
        ChargingJump = true;
        animator.CrossFade("charge", 0, 0);

        while (Input.GetKey(chargeJumpKey))
        {
            yield return null;
        }

        ChargingJump = false;
        animator.CrossFade("jumping", 0, 0);
    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        canJump = true;
    }

    public void TriggerDashingAnimation()
    {
        if (animator != null)
        {
            animator.CrossFade("dash", 0, 0);
        }
    }
}