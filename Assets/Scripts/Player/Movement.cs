using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float horizontal;
    public float speed = 8f;
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

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform playerSprite;
    [SerializeField] private Animator animator;
    private SpringBoots springboots;

    [Header("Audio")]
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource punchSound;
    [SerializeField] private AudioSource chargeSound;

    public bool IsFacingRight => isFacingRight;

    private void Start()
    {
        if (springboots == null)
        {
            springboots = FindObjectOfType<SpringBoots>();
        }
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.B))
        {
            StartBlocking();
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
            StopBlocking();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCrouching();
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            StopCrouching();
        }

        if (isBlocking || isCrouching)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W)) && IsGrounded() && canJump && !recentlyLanded)
        {
            Jump(jumpingPower);
            canDoubleJump = true;
        }
        else if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W)) && canDoubleJump && !IsGrounded())
        {
            Jump(jumpingPower * 0.8f);
            canDoubleJump = false;
        }

        if ((Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.W)) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (Input.GetMouseButtonDown(0) && !isPunching)
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
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        }
    }

    private void StartBlocking()
    {
        isBlocking = true;
        animator.CrossFade("block", 0, 0);
    }

    private void StopBlocking()
    {
        isBlocking = false;
        animator.CrossFade("idle", 0, 0);
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
        bool grounded = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.000001f, groundLayer);
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
        jumpSound?.Play();
        canJump = false;
        StartCoroutine(JumpCooldown());
    }

    private IEnumerator PunchAction()
    {
        isPunching = true;
        punchSound?.Play();
        animator.CrossFade("punch", 0, 0);

        yield return new WaitForSeconds(0.30f);

        isPunching = false;
    }

    private IEnumerator ChargeJump()
    {
        ChargingJump = true;
        chargeSound?.Play();
        animator.CrossFade("charge", 0, 0);

        while (Input.GetKey(KeyCode.LeftAlt))
        {
            ChargingJump = false;
            animator.CrossFade("jumping", 0, 0);
            yield return null;
        }
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
