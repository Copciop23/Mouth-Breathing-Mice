using UnityEngine;
using System.Collections; // Ensure this is included for IEnumerator

public class ShieldController : MonoBehaviour
{
    public GameObject shieldPrefab; // The shield object
    public float shieldDuration = 0.3f; // How long the shield lasts
    public float cooldownTime = 7f; // Cooldown before reusing the shield

    private bool isShieldActive = false;
    private bool isOnCooldown = false;
    private float timeSinceLastShield = 0f; // Track cooldown timer

    // Public property to access the cooldown status
    public bool IsOnCooldown => isOnCooldown;

    // Public property to access the remaining cooldown time
    public float RemainingCooldown => isOnCooldown ? Mathf.Max(0f, cooldownTime - timeSinceLastShield) : 0f;

    void Update()
    {
        // Update the cooldown timer
        if (isOnCooldown)
        {
            timeSinceLastShield += Time.deltaTime;

            // Check if cooldown has finished
            if (timeSinceLastShield >= cooldownTime)
            {
                isOnCooldown = false;
                timeSinceLastShield = 0f; // Reset the timer for the next cooldown
            }
        }

        // Activate shield when 'H' is pressed, if not on cooldown and shield is not active
        if (Input.GetKeyDown(KeyCode.H) && !isShieldActive && !isOnCooldown)
        {
            StartCoroutine(ActivateShield());
        }
    }

    // Coroutine to handle shield activation and cooldown
    IEnumerator ActivateShield()
    {
        isShieldActive = true;
        isOnCooldown = true;

        // Instantiate shield at player's position
        GameObject shield = Instantiate(shieldPrefab, transform.position, Quaternion.identity, transform);

        // Destroy the shield after the duration ends
        yield return new WaitForSeconds(shieldDuration);
        Destroy(shield);

        isShieldActive = false;
    }
}