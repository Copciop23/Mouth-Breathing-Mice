using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject playerSprite;

    private Movement movementScript;
    private PlayerHealth health;
    private PlayerAttributes attributes;
    private PlayerCombatStats combatStats;
    private Vector3 respawnPosition;
    private HashSet<Collider2D> processedColliders = new HashSet<Collider2D>();

    public int Kills => combatStats.Kills;
    public PlayerHealth Health => health;
    public PlayerAttributes Attributes => attributes;
    public PlayerCombatStats CombatStats => combatStats;

    void Start()
    {
        movementScript = GetComponent<Movement>();
        health = new PlayerHealth(100, healthSlider);
        combatStats = new PlayerCombatStats();
        attributes = new PlayerAttributes();

        respawnPosition = transform.position;

        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            Collider2D collider = collision.collider;

            if (!processedColliders.Contains(collider))
            {
                processedColliders.Add(collider);
                health.TakeDamage(20);

                if (health.CurrentHealth <= 0)
                {
                    StartCoroutine(HandleDeath());
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            Collider2D collider = collision.collider;
            processedColliders.Remove(collider);
        }
    }

    private IEnumerator HandleDeath()
    {
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
        }

        if (playerSprite != null)
        {
            playerSprite.SetActive(false);
        }
        movementScript.enabled = false;

        yield return new WaitForSeconds(1);
        movementScript.enabled = true;

        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }

        health.ResetHealth();
        transform.position = respawnPosition;

        if (playerSprite != null)
        {
            playerSprite.SetActive(true);
        }
    }

    public void AddKill()
    {
        combatStats.AddKill();
    }
}

[System.Serializable]
public class PlayerHealth
{
    private int currentHealth;
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