using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint;

    private bool isFacingRight = true;

    void Update()
    {
        // Check for direction change based on player input
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            isFacingRight = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            isFacingRight = true;
        }

        // Adjust fire point position based on direction
        firePoint.localPosition = isFacingRight ? new Vector2(1, 0) : new Vector2(-1, 0);

        // Shoot the fireball when 'F' is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            ShootFireball();
        }
    }

    void ShootFireball()
    {
        // Instantiate the fireball at the fire point position and rotation
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);

        // Get the Rigidbody2D component of the fireball
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Set the velocity of the fireball based on the facing direction
            float direction = isFacingRight ? 1 : -1;
            rb.linearVelocity = new Vector2(direction, 0) * Mathf.Abs(rb.linearVelocity.x);
        }
    }
}
