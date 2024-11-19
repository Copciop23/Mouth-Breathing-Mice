using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireballSpeed = 10f;

    private bool isFacingRight = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            isFacingRight = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            isFacingRight = true;
        }

        firePoint.localPosition = isFacingRight ? new Vector2((float)0.1, 0) : new Vector2((float)-0.1, 0);

        // Shoot the fireball when 'F' is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            ShootFireball();
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
