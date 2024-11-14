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

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform playerSprite;
    [SerializeField] private Animator animator;

    [Header("Audio")]
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource punchSound;

    public bool IsFacingRight => isFacingRight;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W)) && IsGrounded() && canJump && !recentlyLanded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            jumpSound?.Play();
            canJump = false;
            StartCoroutine(JumpCooldown());
        }

        if ((Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.W)) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (Input.GetButtonDown("Fire1") && !isPunching)
        {
            StartCoroutine(PunchAction());
        }

        Flip();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
     rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayer);
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

    private IEnumerator PunchAction()
    {
        isPunching = true;
        punchSound?.Play();
        animator.CrossFade("punch", 0, 0);

        yield return new WaitForSeconds(0.30f);

        isPunching = false;
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
