using UnityEngine;

public class SpitProjectileBehaviour : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 3f;
    public int damage = 20;

    private Rigidbody2D rb;
    [SerializeField] PlayerStats HurtPlayer;

    public bool isFacingRight = true; // Direction from the player, set before firing

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Set velocity based on the direction
            float direction = isFacingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * speed, 0);
        }

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
{
    if (!collision.gameObject.CompareTag("Player")) 
    {
        Destroy(gameObject); 
    }
    else
    {
        PlayerStats playerHealth = collision.gameObject.GetComponent<PlayerStats>();
        if (playerHealth != null)
        {
            HurtPlayer.doDamage(damage);
        }
        Destroy(gameObject);
    }
}
}
