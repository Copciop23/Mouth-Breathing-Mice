using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float stopDistance = 1f;
    public Transform player;

    [Header("Jumping")]
    public float jumpForce = 10f; // Increased for platform penetration
    public float jumpCooldown = 1f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float playerHeightThreshold = 1f; // How much higher player needs to be

    private Rigidbody2D rb;
    private bool isGrounded;
    private float lastJumpTime;

    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        CheckGround();
        HandleMovement();
        TryJumpToPlatform();
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void HandleMovement()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
            if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
            {
                Flip();
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    void TryJumpToPlatform()
    {
        bool playerIsAbove = (player.position.y - transform.position.y) > playerHeightThreshold;
        bool shouldJump = isGrounded && playerIsAbove && Time.time > lastJumpTime + jumpCooldown;

        if (shouldJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            lastJumpTime = Time.time;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);
    }
}