using UnityEngine;
using UnityEngine.UI;

public class EnemyHurt : MonoBehaviour
{
    public int health = 50;
    [SerializeField] private Slider enemyHealthBar;

    private void Start()
    {
        enemyHealthBar.maxValue = health;
        enemyHealthBar.value = health;
    }

    private void Update()
    {
        enemyHealthBar.value = health;
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        health -= damage;

        if (health <= 0)
        {
            AwardKill(attacker);
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void AwardKill(GameObject attacker)
    {
        PlayerStats playerStats = attacker.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.AddKill(); // Increment the player's kill count
        }
    }
}
