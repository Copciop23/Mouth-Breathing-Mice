using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public int playerHealth;
    [SerializeField] Slider slider;
    [SerializeField] private GameObject DeathScreen;
    [SerializeField] private GameObject playerSprite;
    private Movement movementScript;

    private HashSet<Collider2D> processedColliders = new HashSet<Collider2D>();
    private Vector3 respawnPosition;

    void Start()
    {
        movementScript = GetComponent<Movement>();
        playerHealth = 100;
        slider.value = playerHealth;

        respawnPosition = transform.position;

        if (DeathScreen != null)
        {
            DeathScreen.SetActive(false);
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
                doDamage(20);
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

    public void doDamage(int damage)
    {
        if ((playerHealth - damage) <= 0)
        {
            StartCoroutine(HandleDeath());
        }
        else if (playerHealth > 0)
        {
            playerHealth -= damage;
            slider.value = playerHealth;
        }
    }

    private IEnumerator HandleDeath()
    {
        if (DeathScreen != null)
        {
            DeathScreen.SetActive(true);
        }

        if (playerSprite != null)
        {
            playerSprite.SetActive(false);
        }
        movementScript.enabled = false;

        yield return new WaitForSeconds(1);
        movementScript.enabled = true;

        if (DeathScreen != null)
        {
            DeathScreen.SetActive(false);
        }

        playerHealth = 100;
        slider.value = playerHealth;
        transform.position = respawnPosition;

        if (playerSprite != null)
        {
            playerSprite.SetActive(true);
        }
    }
}
