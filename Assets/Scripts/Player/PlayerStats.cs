using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerHealth = 100;
    void Start()
    {
        playerHealth = 100;
    }

    public void TakeDamage(int damage) {
        playerHealth -= damage;
        if (playerHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Debug.Log("Player died. :(");
    }
}
