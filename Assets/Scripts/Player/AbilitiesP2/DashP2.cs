using System.Collections;
using UnityEngine;

public class DashP2 : MonoBehaviour
{
    public float DashPower = 1f;
    public float dashDelay = 1.5f;
    [SerializeField] private Rigidbody2D rb;
    private MovementP2 movement;
    private SpringBootsP2 springboots;
    private PlayerStats playerStats;
    private KeyCode dashKey = KeyCode.Keypad2;

    public bool canDash { get; private set; } = true;
    public float DashTimer { get; private set; }

    void Start()
    {
        movement = GetComponent<MovementP2>();
        springboots = GetComponent<SpringBootsP2>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(dashKey) && canDash && !springboots.IsChargingJump)
        {
            DashAbility();
            StartCoroutine(DashCooldown());
        }

        if (!canDash)
        {
            DashTimer -= Time.deltaTime;
        }
    }

    private void DashAbility()
    {
        movement.TriggerDashingAnimation();
        AudioManager.Instance.PlaySound("dash", AudioManager.AudioType.SFX);
        float dashDirection = movement.IsFacingRight ? DashPower : -DashPower;
        rb.position = new Vector2(rb.position.x + dashDirection, rb.position.y);
    }

    private IEnumerator DashCooldown()
    {
        canDash = false;
        DashTimer = dashDelay;
        yield return new WaitForSeconds(dashDelay);
        canDash = true;
    }
}