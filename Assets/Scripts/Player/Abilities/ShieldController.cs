using System.Collections;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public GameObject shieldPrefab;
    public float shieldDuration = 0.3f;
    public float cooldownTime = 7f;
    public KeyCode shieldKey = KeyCode.H;

    private bool isShieldActive = false;
    private bool isOnCooldown = false;
    private float timeSinceLastShield = 0f;

    public bool IsOnCooldown => isOnCooldown;
    public float RemainingCooldown => isOnCooldown ? Mathf.Max(0f, cooldownTime - timeSinceLastShield) : 0f;

    void Update()
    {
        if (isOnCooldown)
        {
            timeSinceLastShield += Time.deltaTime;
            if (timeSinceLastShield >= cooldownTime)
            {
                isOnCooldown = false;
                timeSinceLastShield = 0f;
            }
        }

        if (Input.GetKeyDown(shieldKey) && !isShieldActive && !isOnCooldown)
        {
            StartCoroutine(ActivateShield());
        }
    }

    IEnumerator ActivateShield()
    {
        isShieldActive = true;
        isOnCooldown = true;

        GameObject shield = Instantiate(shieldPrefab, transform.position, Quaternion.identity, transform);
        yield return new WaitForSeconds(shieldDuration);
        Destroy(shield);

        isShieldActive = false;
    }
}