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

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
