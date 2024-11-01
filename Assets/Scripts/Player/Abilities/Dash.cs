using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float DashPower = 1f;
    public float dashDelay = 1.5f;
    [SerializeField] private Rigidbody2D rb;
    private Movement movement;
    public bool canDash { get; private set; } = true;
    public float DashTimer { get; private set; }

    void Start()
    {
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && canDash)
        {
            dash();
            StartCoroutine(DashCooldown());
        }

        if (!canDash)
        {
            DashTimer -= Time.deltaTime;
        }
    }

    private void dash()
    {
        if (movement == null) return;
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

