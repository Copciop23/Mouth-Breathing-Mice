using UnityEngine;

public class SpitProjectileBehaviour : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 8f;
    public int damage = 20;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.linearVelocity = transform.right * speed;
        }

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.CompareTag("Spike")) {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player")) {
            PlayerStats playerHealth = collision.gameObject.GetComponent<PlayerStats>();
            if (playerHealth != null) {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
