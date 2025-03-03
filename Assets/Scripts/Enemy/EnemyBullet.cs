using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    public float lifetime = 3f;
    public int damage = 10;

    void Start() {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // Assume player has a script with TakeDamage(int damage) method
            other.GetComponent<PlayerStats>()?.doDamage(damage);
            Destroy(gameObject);
        }

        if (other.CompareTag("Shield")) {
            Destroy(gameObject);
        }
    }
}
