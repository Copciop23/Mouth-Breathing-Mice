using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour
{
    public int healthIncrease = 20; // Amount to increase HP

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Ensure your player has the tag "Player"
        {
            PlayerStats playerHealth = collision.GetComponent<PlayerStats>();
            if (playerHealth != null)
            {
                playerHealth.Health.Heal(20);
            }

            Destroy(gameObject); // Remove power-up after collection
        }
    }
}
