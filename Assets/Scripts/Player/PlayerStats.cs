using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Death Screen")]
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private TMP_Text wastedText;
    [SerializeField] private Image fadeImage;

    [Header("Death Screen Settings")]
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float wastedDisplayTime = 1.5f;

    [Header("Other Attributes")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Slider healthSlider;
    private Movement movementP1;
    private MovementP2 movementP2;
    private PlayerHealth health;
    private PlayerAttributes attributes;
    private PlayerCombatStats combatStats;
    private bool isDead;
    private HashSet<Collider2D> processedColliders = new HashSet<Collider2D>();

    // Now you can directly set the player number in the Inspector
    [SerializeField] public int playerNumber;  // 1 for Player 1, 2 for Player 2

    public int Kills => combatStats.Kills;
    public PlayerHealth Health => health;
    public PlayerAttributes Attributes => attributes;
    public PlayerCombatStats CombatStats => combatStats;

    void Start()
    {
        movementP1 = GetComponent<Movement>();
        movementP2 = GetComponent<MovementP2>();

        health = new PlayerHealth(100, healthSlider);
        combatStats = new PlayerCombatStats();
        attributes = new PlayerAttributes();

        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }

        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0);
        }
    }

    void Update()
    {
        if (!isDead && health != null && health.CurrentHealth <= 0)
        {
            isDead = true;
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {

        if (movementP1 != null) movementP1.enabled = false;
        if (movementP2 != null) movementP2.enabled = false;

        if (deathScreen != null)
        {
            deathScreen.SetActive(true);

            // Show text and image
            if (wastedText != null && fadeImage != null)
            {
                float timer = 0f;
                wastedText.gameObject.SetActive(true);
                while (timer < fadeDuration)
                {
                    timer += Time.deltaTime;
                    float alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
                    fadeImage.color = new Color(0, 0, 0, alpha);
                    yield return null;
                }
                yield return new WaitForSeconds(wastedDisplayTime);
                wastedText.gameObject.SetActive(false);
            }

            // Wait AFTER fade completes
            yield return new WaitForSeconds(0.5f);

            if (movementP1 != null) movementP1.enabled = true;
            if (movementP2 != null) movementP2.enabled = true;

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
        yield break;
    }

    public void AddKill()
    {
        combatStats.AddKill();
    }
}

[System.Serializable]
public class PlayerHealth
{
    private int currentHealth = 100;
    private int maxHealth;
    private Slider healthSlider;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public PlayerHealth(int maxHealth, Slider healthSlider)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
        this.healthSlider = healthSlider;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateHealthSlider();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthSlider();
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        UpdateHealthSlider();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }
}

[System.Serializable]
public class PlayerAttributes
{
    private float jumpPower = 4f;
    private float punchPower = 10f;
    private float speed = 3f;

    public float JumpPower
    {
        get => jumpPower;
        set => jumpPower = Mathf.Max(0, value);
    }

    public float PunchPower
    {
        get => punchPower;
        set => punchPower = Mathf.Max(0, value);
    }

    public float Speed
    {
        get => speed;
        set => speed = Mathf.Max(0, value);
    }

    public void IncreaseJumpPower(float amount) => JumpPower += amount;
    public void IncreasePunchPower(float amount) => PunchPower += amount;
    public void IncreaseSpeed(float amount) => Speed += amount;
}

[System.Serializable]
public class PlayerCombatStats
{
    private int kills;

    public int Kills => kills;

    public void AddKill()
    {
        kills++;
    }

    public void ResetKills()
    {
        kills = 0;
    }
}