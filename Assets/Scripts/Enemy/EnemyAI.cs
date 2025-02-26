using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public Transform player;  // Assign in Inspector or find in Start()
    public float moveSpeed = 3f;
    public float chaseRange = 8f;
    public float stopRange = 2f;  // Stops moving when close
    public float jumpForce = 6f;
    public Transform groundCheck;  // Empty GameObject under enemy
    public LayerMask groundLayer;
    public Rigidbody2D rb;

    private bool isGrounded;
    private bool facingRight = true;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseRange && distanceToPlayer > stopRange) {
            MoveTowardsPlayer();
        } else {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        FlipSprite();
    }

    void MoveTowardsPlayer() {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        // Jump over obstacles (checks if grounded and near a wall)
        if (isGrounded && IsNearWall()) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    bool IsNearWall() {
        return Physics2D.Raycast(transform.position, Vector2.right * (facingRight ? 1 : -1), 0.5f, groundLayer);
    }

    void FlipSprite() {
        if ((player.position.x > transform.position.x && !facingRight) || 
            (player.position.x < transform.position.x && facingRight)) {
            facingRight = !facingRight;
            transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);
        }
    }
}
