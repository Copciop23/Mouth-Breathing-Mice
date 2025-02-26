using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireballSpeed = 10f;
    public float cooldownTime = 5f;  // Set cooldown in the Inspector

    private bool isFacingRight = true;
    private float timeSinceLastShot = 0f;
    private bool isOnCooldown = false;

    // Public property to access the cooldown status
    public bool IsOnCooldown => isOnCooldown;

    // Public property to access the remaining cooldown time
    public float RemainingCooldown => isOnCooldown ? Mathf.Max(0f, cooldownTime - timeSinceLastShot) : 0f;

    void Update()
    {
        // Update the cooldown timer
        if (isOnCooldown)
        {
            timeSinceLastShot += Time.deltaTime;

            // Check if cooldown has finished
            if (timeSinceLastShot >= cooldownTime)
            {
                isOnCooldown = false;
                timeSinceLastShot = 0f; // Reset the timer for the next cooldown
            }
        }

        // Handle player input for direction
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            isFacingRight = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            isFacingRight = true;
        }

        // Update fireball spawn position based on direction
        firePoint.localPosition = isFacingRight ? new Vector2(0.1f, 0) : new Vector2(-0.1f, 0);

        // Shoot the fireball when 'F' is pressed and cooldown is not active
        if (Input.GetKeyDown(KeyCode.F) && !isOnCooldown)
        {
            ShootFireball();
            isOnCooldown = true; // Start cooldown
        }
    }

    void ShootFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);

        SpitProjectileBehaviour projectileBehaviour = fireball.GetComponent<SpitProjectileBehaviour>();
        if (projectileBehaviour != null)
        {
            projectileBehaviour.isFacingRight = isFacingRight;
        }
        AudioManager.Instance.PlaySound("fireball", AudioManager.AudioType.SFX);
    }
}