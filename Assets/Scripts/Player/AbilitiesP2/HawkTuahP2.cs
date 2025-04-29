using UnityEngine;

public class HawkTuahP2 : MonoBehaviour
{
    public GameObject spitPrefab;
    public Transform firePoint;
    public float spitSpeed = 5f;
    public float cooldownTime = 3f;
    public KeyCode spitKey = KeyCode.Keypad5;

    private bool isFacingRight = true;
    private float timeSinceLastShot = 0f;
    private bool isOnCooldown = false;

    public bool IsOnCooldown => isOnCooldown;
    public float RemainingCooldown => isOnCooldown ? Mathf.Max(0f, cooldownTime - timeSinceLastShot) : 0f;

    void Update()
    {
        if (isOnCooldown)
        {
            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot >= cooldownTime)
            {
                isOnCooldown = false;
                timeSinceLastShot = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isFacingRight = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            isFacingRight = true;
        }

        firePoint.localPosition = isFacingRight ? new Vector2(0.5f, 0f) : new Vector2(-0.5f, 0f);

        if (Input.GetKeyDown(spitKey) && !isOnCooldown)
        {
            ShootSpit();
            isOnCooldown = true;
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
