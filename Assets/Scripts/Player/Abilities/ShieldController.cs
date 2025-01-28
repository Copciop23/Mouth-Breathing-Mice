using System.Collections;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public GameObject shieldPrefab;  // The bubble shield prefab
    private GameObject activeShield; // Reference to the active shield
    public float shieldDuration = 0.5f;  // How long the shield lasts
    private bool isShieldActive = false;  // Track whether the shield is active

    public float cooldownTime = 7f;  // Cooldown time in seconds
    private float timeSinceLastShield = 0f;  // Timer for cooldown

    void Update()
    {
        // Update the cooldown timer
        timeSinceLastShield += Time.deltaTime;

        // If 'H' key is pressed and shield is not active and cooldown has finished
        if (Input.GetKeyDown(KeyCode.H) && !isShieldActive && timeSinceLastShield >= cooldownTime)
        {
            ActivateShield();
            timeSinceLastShield = 0f;  // Reset the cooldown timer
        }
    }

    // Activate the shield
    void ActivateShield()
    {
        isShieldActive = true;

        // Instantiate the shield at the player's position
        activeShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
        activeShield.transform.SetParent(transform);  // Keep shield at the player

        // Add a collider to the shield
        CircleCollider2D collider = activeShield.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;  // Set it to trigger mode, so it doesn't physically block the player

        // Destroy the shield after the specified duration
        Destroy(activeShield, shieldDuration);

        // Disable the shield after duration
        StartCoroutine(DisableShieldAfterDelay());
    }

    // Coroutine to disable the shield after a delay
    private IEnumerator DisableShieldAfterDelay()
    {
        yield return new WaitForSeconds(shieldDuration);
        isShieldActive = false;
    }

    // Collision detection with projectiles
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isShieldActive && other.CompareTag("Projectile"))
        {
            // Destroy the projectile if it hits the shield
            Destroy(other.gameObject);
        }
    }
}
