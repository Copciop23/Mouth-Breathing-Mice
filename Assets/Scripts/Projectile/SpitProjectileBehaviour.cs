using UnityEngine;

public class SpitProjectileBehaviour : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 8f;
    public int damage = 20;

    private Rigidbody2D rb;
    [SerializeField] private PlayerStats HurtPlayer;

    public bool isFacingRight { get; set; }
    private PlayerStats shooterStats;  // Add a reference to the player who shot this projectile

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set the player reference at the moment of firing the projectile
        shooterStats = GetComponentInParent<PlayerStats>(); // Gets the parent player stats

        if (rb != null)
        {
            float direction = isFacingRight ? 1f : -1f; // Set direction based on facing
            rb.linearVelocity = new Vector2(direction * speed, 0f); // Apply velocity in the correct direction
        }

        Destroy(gameObject, lifetime);  // Destroy the projectile after its lifetime
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHurt enemyHP = collision.gameObject.GetComponent<EnemyHurt>();
            if (enemyHP != null)
            {
                enemyHP.TakeDamage(damage, gameObject);
            }
        }

        // Ignore collisions with other projectiles
        if (collision.gameObject.CompareTag("Projectile"))
        {
            return; // Do nothing, let it pass through
        }

        // Ignore collisions with the shield
        if (collision.gameObject.CompareTag("Shield"))
        {
            return; // Do nothing, let it pass through
        }

        // If it hits a player, deal damage and destroy the projectile
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerHealth = collision.gameObject.GetComponent<PlayerStats>();
            if (playerHealth != null && playerHealth != shooterStats) // Make sure we don't damage the shooter
            {
                playerHealth.Health.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        // Destroy on any other collision (e.g., walls, spikes, environment)
        else
        {
            Destroy(gameObject);
        }
    }
}