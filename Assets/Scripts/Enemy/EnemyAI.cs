using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public Transform player;  
    public float moveSpeed = 3f;
    public float raycastDistance = 1f;
    public float chaseRange = 8f;
    public float stopRange = 2f;
    public float jumpForce = 6f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Rigidbody2D rb;

    private bool isGrounded;
    private bool facingRight = true;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        float distanceToPlayer = Mathf.Abs(player.position.x - rb.position.x);

        if (distanceToPlayer < chaseRange && distanceToPlayer > stopRange) {
            MoveTowardsPlayer();
        } else {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        FlipSprite();
    }

    void MoveTowardsPlayer() {
        float direction = Mathf.Sign(player.position.x - rb.position.x);
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        // Jump on platforms if player is above
        if (isGrounded && IsNearEdge() && player.position.y > transform.position.y) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Jump over obstacles
        if (isGrounded && IsNearWall()) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    bool IsNearEdge() {
        Vector2 rayOrigin = rb.position + new Vector2(facingRight ? 0.5f : -0.5f, 0);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, raycastDistance, groundLayer);
        return hit.collider == null;
    }

    bool IsNearWall() {
        return Physics2D.Raycast(rb.position, Vector2.right * (facingRight ? 1 : -1), 0.5f, groundLayer);
    }

    void FlipSprite() {
        if ((rb.linearVelocity.x > 0 && !facingRight) || (rb.linearVelocity.x < 0 && facingRight)) {
            facingRight = !facingRight;
            transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);
        }
    }
}
