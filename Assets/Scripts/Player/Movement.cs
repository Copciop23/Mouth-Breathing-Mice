using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float horizontal;
    public float speed = 8f;
    public float jumpingPower = 8f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform playerSprite;
    [SerializeField] private Animator animator;

    [Header("Audio")]
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource punchSound;
    [SerializeField] private AudioSource dashSound;

    public bool IsFacingRight => isFacingRight;
    public PlayerState currentState;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        UpdateState();

        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W)) && isGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            jumpSound?.Play();
        }
        if ((Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.W)) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            currentState = PlayerState.Punching;
            punchSound?.Play();
        }

        flip();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
    }

    private void flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = playerSprite.localScale;
            localScale.x *= -1f;
            playerSprite.localScale = localScale;
        }
    }

    private void UpdateState()
    {
        if (currentState == PlayerState.Punching) return;

        if (!isGrounded() && rb.linearVelocity.y > 0)
        {
            currentState = PlayerState.Jumping;
        }
        else if (rb.linearVelocity.y < 0)
        {
            currentState = PlayerState.Landing;
        }
        else if (horizontal != 0 && isGrounded())
        {
            currentState = PlayerState.Running;
        }
        else
        {
            currentState = PlayerState.Idle;
        }

        animator.SetBool("isRunning", currentState == PlayerState.Running);
        animator.SetBool("isJumping", currentState == PlayerState.Jumping);
        animator.SetBool("isLanding", currentState == PlayerState.Landing);

        Debug.Log($"Current State: {currentState}");
    }

    public enum PlayerState
    {
        Idle,
        Running,
        Jumping,
        Landing,
        Dashing,
        Punching
    }

    public void EndPunchingState()
    {
        currentState = PlayerState.Idle;
    }
}
