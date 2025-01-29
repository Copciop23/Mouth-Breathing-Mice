using UnityEngine;

public class HawkTuah : MonoBehaviour
{
    public GameObject spitPrefab;
    public Transform firePoint;
    public float spitSpeed = 5f;
    public float cooldownTime = 3f;  // Set cooldown in the Inspector

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

        // Shoot the spit when 'G' is pressed and cooldown has passed
        if (Input.GetKeyDown(KeyCode.G) && timeSinceLastShot >= cooldownTime)
        {
            ShootSpit();
            timeSinceLastShot = 0f;  // Reset cooldown timer
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
