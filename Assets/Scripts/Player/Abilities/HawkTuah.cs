using UnityEngine;

public class HawkTuah : MonoBehaviour
{
    public GameObject spitPrefab; // Prefab for the spit projectile
    public Transform firePoint; // Position from where the projectile is fired
    public float spitSpeed = 5f; // Speed of the projectile

    private bool isFacingRight = true;

    void Update()
    {
        // Update facing direction based on input
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            isFacingRight = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            isFacingRight = true;
        }

        // Update fire point position based on facing direction
        firePoint.localPosition = isFacingRight ? new Vector2(0.1f, 0) : new Vector2(-0.1f, 0);

        // Shoot the spit when 'G' is pressed
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Check cooldown before shooting
            SpitProjectileBehaviour spitBehaviour = spitPrefab.GetComponent<SpitProjectileBehaviour>();
            if (spitBehaviour != null && spitBehaviour.TryUseSkill())
            {
                ShootSpit();
            }
            else
            {
                Debug.Log("Skill is on cooldown!");
            }
        }
    }

    void ShootSpit()
    {
        // Instantiate the spit projectile at the fire point
        GameObject spit = Instantiate(spitPrefab, firePoint.position, firePoint.rotation);

        // Set the direction of the spit projectile
        SpitProjectileBehaviour projectileBehaviour = spit.GetComponent<SpitProjectileBehaviour>();
        if (projectileBehaviour != null)
        {
            projectileBehaviour.isFacingRight = isFacingRight;
        }
    }
}
