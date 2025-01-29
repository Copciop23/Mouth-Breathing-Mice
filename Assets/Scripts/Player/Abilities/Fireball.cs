using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireballSpeed = 10f;
    public float cooldownTime = 5f;  // Set cooldown in the Inspector

    private bool isFacingRight = true;
    private float timeSinceLastShot = 0f; // Track cooldown timer

    void Update()
    {
        timeSinceLastShot += Time.deltaTime; // Update cooldown timer

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            isFacingRight = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            isFacingRight = true;
        }

        firePoint.localPosition = isFacingRight ? new Vector2(0.1f, 0) : new Vector2(-0.1f, 0);

        // Shoot the fireball when 'F' is pressed and cooldown has passed
        if (Input.GetKeyDown(KeyCode.F) && timeSinceLastShot >= cooldownTime)
        {
            ShootFireball();
            timeSinceLastShot = 0f;  // Reset cooldown timer
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
    }
}
