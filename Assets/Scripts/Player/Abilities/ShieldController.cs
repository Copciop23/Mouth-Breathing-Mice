using UnityEngine;
using System.Collections;

public class ShieldController : MonoBehaviour
{
    public GameObject shieldPrefab; // The shield object
    public float shieldDuration = 0.3f; // How long the shield lasts
    public float cooldownTime = 7f; // Cooldown before reusing the shield

    private bool isShieldActive = false;
    private bool isOnCooldown = false;

    void Update()
    {
        // Activate shield when 'H' is pressed, if not on cooldown
        if (Input.GetKeyDown(KeyCode.H) && !isShieldActive && !isOnCooldown)
        {
            StartCoroutine(ActivateShield());
        }
    }

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

        // Wait for cooldown before allowing another shield activation
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }
}
