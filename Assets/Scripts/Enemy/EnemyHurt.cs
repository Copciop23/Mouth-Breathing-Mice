using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHurt : MonoBehaviour
{
    [SerializeField] public int health;
    [SerializeField] public int maxHealth;
    [SerializeField] private Slider enemyHealthBar;
    [SerializeField] private GameManager gameManager;
    private bool isDead;
    [Header("Death Screen")]
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private TMP_Text bossDefeatedText;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float wastedDisplayTime = 1.5f;
    private EnemyAI movement;
    private EnemyShooting enemyShooting;
    private void Start()
    {
        movement = GetComponent<EnemyAI>();
        enemyShooting = GetComponent<EnemyShooting>();

        enemyHealthBar.maxValue = health;
        enemyHealthBar.value = health;

        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }
    }

    private void Update()
    {
        enemyHealthBar.value = health;

        if (!isDead && health <= 0)
        {
            isDead = true;
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        // Turn off moving and shooting of AI
        if (movement != null) movement.enabled = false;
        if (enemyShooting != null) enemyShooting.enabled = false;

        if (deathScreen != null)
        {
            movement.enabled = false;

            deathScreen.SetActive(true);

            // Show text and image
            if (bossDefeatedText != null && fadeImage != null)
            {
                float timer = 0f;
                bossDefeatedText.gameObject.SetActive(true);
                while (timer < fadeDuration)
                {
                    timer += Time.deltaTime;
                    float alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
                    fadeImage.color = new Color(0, 0, 0, alpha);
                    yield return null;
                }
                yield return new WaitForSeconds(wastedDisplayTime);
                bossDefeatedText.gameObject.SetActive(false);
            }

            // Wait AFTER fade completes
            yield return new WaitForSeconds(0.5f);

            // Turn the movement and shooting back on
            if (movement != null) movement.enabled = true;
            if (enemyShooting != null) enemyShooting.enabled = true;

            // Return to menu AFTER all visual effects
            if (gameManager != null)
            {

                gameManager.ReturnToMenu();
            }
            else
            {
                Debug.LogError("GameManager reference missing!");
            }
            deathScreen.SetActive(false);
        }

        isDead = false;
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        health -= damage;

        if (health <= 0)
        {
            AwardKill(attacker);
        }
    }

    public void resetHealth()
    {
        health = maxHealth;
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