using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public Transform player;
    public float moveSpeed = 3f;
    public float raycastDistance = 1f;
    public float chaseRange = 8f;
    public float stopRange = 2f;
    public float jumpForce = 6f;
    public float verticalCheckDistance = 1.5f; // How much higher to check for player
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Rigidbody2D rb;
    public float maxHealth = 100f;
    public float currentHealth;
    
    private bool isGrounded;
    private bool facingRight = true;
    private float nextJumpTime = 0f;
    private float jumpCooldown = 1f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        
        float distanceToPlayer = Vector2.Distance(player.position, rb.position);
        
        if (distanceToPlayer < chaseRange && distanceToPlayer > stopRange) {
            ChasePlayer();
        } else {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        
        FlipSprite();
    }

    void ChasePlayer() {
        // Horizontal movement
        float direction = Mathf.Sign(player.position.x - rb.position.x);
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        
        // Check if player is above and we should jump
        bool playerIsHigher = player.position.y > transform.position.y + 0.5f;
        bool playerIsLower = player.position.y < transform.position.y - 0.5f;
        
        // Jump conditions
        if (isGrounded && Time.time > nextJumpTime) {
            // Jump if player is significantly higher and we're near them horizontally
            if (playerIsHigher && Mathf.Abs(player.position.x - transform.position.x) < 2f) {
                Jump();
            }
            // Jump if we're at an edge and player is not way below us
            else if (IsNearEdge() && !playerIsLower) {
                Jump();
            }
            // Jump if there's a wall in our path
            else if (IsNearWall()) {
                Jump();
            }
        }
    }
    
    void Jump() {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        nextJumpTime = Time.time + jumpCooldown;
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
    }

    bool IsNearEdge() {
        Vector2 rayOrigin = rb.position + new Vector2(facingRight ? 0.5f : -0.5f, 0);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, raycastDistance, groundLayer);
        return hit.collider == null;
    }

    bool IsNearWall() {
        Vector2 rayDirection = facingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(rb.position, rayDirection, 0.5f, groundLayer);
        return hit.collider != null;
    }

    void FlipSprite() {
        if ((rb.linearVelocity.x > 0 && !facingRight) || (rb.linearVelocity.x < 0 && facingRight)) {
            facingRight = !facingRight;
            transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);
        }
    }
}