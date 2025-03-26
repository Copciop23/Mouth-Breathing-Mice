using UnityEngine;

public class EnemyAI : MonoBehaviour {
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float chaseRange = 8f;
    public float stopRange = 2f;
    public float deadZone = 0.5f; // Minimum distance to consider changing direction
    
    [Header("Jump Settings")]
    public float jumpForce = 6f;
    public float raycastDistance = 1f;
    public float verticalCheckDistance = 1.5f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    
    [Header("Combat Settings")]
    public float maxHealth = 100f;
    
    [Header("References")]
    public Transform player;
    public Rigidbody2D rb;
    
    private bool isGrounded;
    private bool facingRight = true;
    private bool isJumping = false;
    private float currentHealth;
    private float lastFlipTime;
    private float flipCooldown = 0.2f; // Minimum time between flips

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update() {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        
        if (!wasGrounded && isGrounded) {
            isJumping = false;
        }
        
        float distanceToPlayer = Vector2.Distance(player.position, rb.position);
        
        if (distanceToPlayer < chaseRange && distanceToPlayer > stopRange) {
            ChasePlayer();
        } else {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    void ChasePlayer() {
        float horizontalDistance = player.position.x - transform.position.x;
        
        // Only move if player is outside dead zone
        if (Mathf.Abs(horizontalDistance) > deadZone) {
            float direction = Mathf.Sign(horizontalDistance);
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
            TryFlipSprite(direction);
        } else {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        
        // Jump logic remains the same
        if (isGrounded && !isJumping) {
            bool playerIsHigher = player.position.y > transform.position.y + 0.5f;
            bool playerIsLower = player.position.y < transform.position.y - 0.5f;
            
            if (playerIsHigher && Mathf.Abs(horizontalDistance) < 2f) {
                Jump();
            }
            else if (IsNearEdge() && !playerIsLower) {
                Jump();
            }
            else if (IsNearWall()) {
                Jump();
            }
        }
    }
    
    void TryFlipSprite(float direction) {
        // Only flip if enough time has passed and direction actually changed
        if (Time.time > lastFlipTime + flipCooldown) {
            bool shouldFaceRight = direction > 0;
            if (shouldFaceRight != facingRight) {
                facingRight = shouldFaceRight;
                transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);
                lastFlipTime = Time.time;
            }
        }
    }
    
    void Jump() {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isJumping = true;
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    void Die() { Destroy(gameObject); }

    bool IsNearEdge() {
        Vector2 rayOrigin = rb.position + new Vector2(facingRight ? 0.5f : -0.5f, 0);
        return !Physics2D.Raycast(rayOrigin, Vector2.down, raycastDistance, groundLayer);
    }

    bool IsNearWall() {
        Vector2 rayDirection = facingRight ? Vector2.right : Vector2.left;
        return Physics2D.Raycast(rb.position, rayDirection, 0.5f, groundLayer);
    }
}