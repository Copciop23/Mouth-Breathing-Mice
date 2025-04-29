using UnityEngine;

public class FireballP2 : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireballSpeed = 10f;
    public float cooldownTime = 5f;
    public KeyCode fireKey = KeyCode.Keypad3;

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

        if (Input.GetKeyDown(fireKey) && !isOnCooldown)
        {
            ShootFireball();
            isOnCooldown = true;
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