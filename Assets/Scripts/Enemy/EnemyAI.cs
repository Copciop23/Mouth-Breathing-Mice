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
    public float maxHealth = 100f;
    public float currentHealth;
    public float lowHealthThreshold = 0.3f; // 30% health
    public float healAmount = 30f;
    public GameObject[] powerups; // Array of powerups in the scene

    private bool isGrounded;
    private bool facingRight = true;
    private bool isRunningAway = false;
    private GameObject targetPowerup;

    void Start() {
    rb = GetComponent<Rigidbody2D>();
    currentHealth = maxHealth;
    powerups = GameObject.FindGameObjectsWithTag("Powerup"); // Find all powerups with the "Powerup" tag
}

    void Update() {
    isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

    if (currentHealth <= maxHealth * lowHealthThreshold) {
        Debug.Log("Enemy health is low! Running away.");
        isRunningAway = true;
        if (targetPowerup == null) {
            FindNearestPowerup();
        } else {
            RunAwayAndHeal();
        }
    } else {
        isRunningAway = false;
        float distanceToPlayer = Mathf.Abs(player.position.x - rb.position.x);

        if (distanceToPlayer < chaseRange && distanceToPlayer > stopRange) {
            Debug.Log("Chasing player.");
            MoveTowardsPlayer();
        } else {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    FlipSprite();
}

    void MoveTowardsPlayer() {
    float direction = Mathf.Sign(player.position.x - rb.position.x);
    rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

    // Jump if near an edge or wall
    if (isGrounded) {
        if (IsNearEdge() && player.position.y > transform.position.y) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        if (IsNearWall()) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}

    void RunAwayAndHeal() {
    if (targetPowerup == null) return;

    // Calculate direction to powerup
    float powerupDirection = Mathf.Sign(targetPowerup.transform.position.x - transform.position.x);
    rb.linearVelocity = new Vector2(powerupDirection * moveSpeed, rb.linearVelocity.y);

    // Jump if near an edge or wall
    if (isGrounded) {
        if (IsNearEdge() && player.position.y > transform.position.y) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        if (IsNearWall()) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // Check if reached the powerup
    if (Vector2.Distance(transform.position, targetPowerup.transform.position) < 1f) {
        Heal();
    }
}

    void FindNearestPowerup() {
    float nearestDistance = Mathf.Infinity;
    foreach (GameObject powerup in powerups) {
        float distance = Vector2.Distance(transform.position, powerup.transform.position);
        if (distance < nearestDistance) {
            nearestDistance = distance;
            targetPowerup = powerup;
        }
    }
    Debug.Log($"Nearest powerup found: {targetPowerup}");
}

    void Heal() {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        targetPowerup = null; // Reset target powerup after healing
        isRunningAway = false; // Stop running away after healing
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        // Handle enemy death (e.g., play animation, destroy object, etc.)
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