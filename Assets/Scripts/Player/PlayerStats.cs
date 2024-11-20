using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    public int playerHealth;
    [SerializeField] Slider slider;

    private HashSet<Collider2D> processedColliders = new HashSet<Collider2D>();
    private Vector3 respawnPosition; // Variable to store the respawn position

    void Start()
    {
        playerHealth = 100;
        slider.value = playerHealth;

        // Save the player's initial position as the respawn position
        respawnPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            Collider2D collider = collision.collider;

            if (!processedColliders.Contains(collider))
            {
                processedColliders.Add(collider);
                doDamage(20);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            Collider2D collider = collision.collider;

            // Remove the collider from the processed set when the collision ends
            processedColliders.Remove(collider);
        }
    }

    public void doDamage(int damage)
    {
        if ((playerHealth-damage) <= 0)
        {
            Die();
        }
        else if (playerHealth > 0)
        {
            playerHealth -= damage;
            slider.value = playerHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player died. :(");

        // Reset health and slider value
        playerHealth = 100;
        slider.value = playerHealth;

        // Move player back to the respawn position
        transform.position = respawnPosition;
    }
}
