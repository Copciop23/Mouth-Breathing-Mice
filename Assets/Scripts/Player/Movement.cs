using System.Collections;
using System.Collections.Generic;
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

    public bool IsFacingRight => isFacingRight;

    // State tracking
    public PlayerState currentState;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Update state based on input
        UpdateState();

        if (Input.GetButtonDown("Jump") && isGrounded() || Input.GetKeyDown(KeyCode.W) && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f || Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        flip(); // Flip the sprite
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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

        if (!isGrounded() && rb.velocity.y >=0)
        {
            currentState = PlayerState.Jumping;
        }
        else if (rb.velocity.y < 0)
        {
            currentState = PlayerState.Landing;
        }
        else if (horizontal != 0 && rb.velocity.y >= 0)
        {
            currentState = PlayerState.Running;
        }
        else
        {
            currentState = PlayerState.Idle;
        }


        switch (currentState)
        {
            case PlayerState.Idle:
                //animator.SetTrigger("Idle");
                break;
            case PlayerState.Running:
                Debug.Log("Running");
                break;
            case PlayerState.Jumping:
                Debug.Log("Jumping");
                break;
            case PlayerState.Landing:
                Debug.Log("Landing");
                break;
            case PlayerState.Dashing:
                Debug.Log("Dashing");
                break;
        }
    }
    public enum PlayerState
    {
        Idle,
        Running,
        Jumping,
        Landing,
        Dashing
    }
}


