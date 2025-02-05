using UnityEngine;

public class HawkTuah : MonoBehaviour
{
    public GameObject spitPrefab;
    public Transform firePoint;
    public float spitSpeed = 5f;
    public float cooldownTime = 3f;  // Set cooldown in the Inspector

    private bool isFacingRight = true;
    private float timeSinceLastShot = 0f; // Track cooldown timer
    private bool isOnCooldown = false; // Track cooldown state

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

        // Update spit spawn position based on direction
        firePoint.localPosition = isFacingRight ? new Vector2(0.1f, 0) : new Vector2(-0.1f, 0);

        // Shoot the spit when 'G' is pressed and cooldown is not active
        if (Input.GetKeyDown(KeyCode.G) && !isOnCooldown)
        {
            ShootSpit();
            isOnCooldown = true; // Start cooldown
        }
    }

    void ShootSpit()
    {
        GameObject spit = Instantiate(spitPrefab, firePoint.position, firePoint.rotation);

        SpitProjectileBehaviour projectileBehaviour = spit.GetComponent<SpitProjectileBehaviour>();
        if (projectileBehaviour != null)
        {
            projectileBehaviour.isFacingRight = isFacingRight;
        }
    }
}