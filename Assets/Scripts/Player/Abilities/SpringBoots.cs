using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpringBoots : MonoBehaviour
{
    public float baseJumpPower = 4f;
    public float maxJumpPower = 8f;
    public float jumpChargeRate = 2f;
    private float currentJumpPower;

    private bool isChargingJump = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private Slider ChargeSlider;
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
        if (Input.GetKeyDown(KeyCode.LeftControl) && IsGrounded())
        {
            isChargingJump = true;
            currentJumpPower = baseJumpPower;
        }

        // Charge jump power while Ctrl is held
        if (isChargingJump && Input.GetKey(KeyCode.LeftControl))
        {
            currentJumpPower += jumpChargeRate * Time.deltaTime;
            currentJumpPower = Mathf.Clamp(currentJumpPower, baseJumpPower, maxJumpPower);
        }

        // Release the jump when Ctrl is released
        if (Input.GetKeyUp(KeyCode.LeftControl) && isChargingJump)
        {
            PerformJump(currentJumpPower);
            isChargingJump = false;
        }
        ChargeSlider.value = currentJumpPower;
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayer);
    }

    private void PerformJump(float power)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, power);
        jumpSound?.Play();
        currentJumpPower = 0;
    }
}
