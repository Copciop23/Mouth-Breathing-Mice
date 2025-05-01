using UnityEngine;

public class SpringBootsP2 : MonoBehaviour
{
    public float baseJumpPower = 4f;
    public float maxJumpPower = 8f;
    public float jumpChargeRate = 2f;
    private float currentJumpPower;
    private bool isChargingJump = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private KeyCode chargeKey = KeyCode.RightControl;

    public float CurrentJumpPower => currentJumpPower;
    public bool IsChargingJump => isChargingJump;

    private void Update()
    {
        if (isChargingJump)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (Input.GetKeyDown(chargeKey) && IsGrounded())
        {
            isChargingJump = true;
            currentJumpPower = baseJumpPower;
        }

        if (isChargingJump && Input.GetKey(chargeKey))
        {
            currentJumpPower += jumpChargeRate * Time.deltaTime;
            currentJumpPower = Mathf.Clamp(currentJumpPower, baseJumpPower, maxJumpPower);
        }

        if (Input.GetKeyUp(chargeKey) && isChargingJump)
        {
            PerformJump(currentJumpPower);
            isChargingJump = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayer);
    }

    private void PerformJump(float power)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, power);
        AudioManager.Instance.PlaySound("jump", AudioManager.AudioType.SFX);
    }
}