using UnityEngine;

public class SpitProjectileBehaviour : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 8f;
    public int damage = 20;

    public float cooldown = 1f; // Cooldown time in seconds

    private Rigidbody2D rb;
    [SerializeField] PlayerStats HurtPlayer;
    private static float lastUsedTime = -Mathf.Infinity;

    public bool isFacingRight { get; set; }

void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null) {
            float direction = isFacingRight ? 1f : -1f; // Set direction based on facing
            rb.linearVelocity = new Vector2(direction * speed, 0f);
        }

        Destroy(gameObject, lifetime);
    }

    public bool IsReady() {
        return Time.time >= lastUsedTime + cooldown;
    }

    public bool TryUseSkill() {
        if (IsReady()) {
            lastUsedTime = Time.time;
            return true;
        } else {
            return false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
{
     if (collision.gameObject.CompareTag("Spike"))
        {
            Destroy(gameObject);
        }
        // If it hits a player, deal damage and destroy the projectile
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerHealth = collision.gameObject.GetComponent<PlayerStats>();
            if (playerHealth != null)
            {
                HurtPlayer.doDamage(damage);
            }
            Destroy(gameObject);
        }
        // For all other collisions, just destroy the projectile
        else
        {
            Destroy(gameObject);
        }
}
}
