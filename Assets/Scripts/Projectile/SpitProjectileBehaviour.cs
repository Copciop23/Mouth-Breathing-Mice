using UnityEditor.Tilemaps;
using UnityEngine;

public class SpitProjectileBehaviour : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 8f;
    public int damage = 20;

    private Rigidbody2D rb;
    [SerializeField] PlayerStats HurtPlayer;

    public bool isFacingRight { get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float direction = isFacingRight ? 1f : -1f; // Set direction based on facing
            rb.linearVelocity = new Vector2(direction * speed, 0f);
        }

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) {
            EnemyHurt enemyHP = collision.gameObject.GetComponent<EnemyHurt>();
            if (enemyHP != null) {
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
            if (playerHealth != null)
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
