using System;
using System.Collections;
using UnityEngine;

public class ShieldControllerP2 : MonoBehaviour
{
    [Header("Settings")]
    public float shieldDuration = 0.3f;
    public float cooldownTime = 7f;
    public KeyCode shieldKey = KeyCode.Keypad4;
    public Vector2 shieldOffset = new Vector2(0.5f, 0f);
    public float shieldRadius = 1f;
    [Range(0.1f, 1f)] public float shieldOpacity = 0.5f;

    [Header("State")]
    [SerializeField] private bool isShieldActive = false;
    [SerializeField] private bool isOnCooldown = false;
    [SerializeField] private float timeSinceLastShield = 0f;
    private GameObject shieldVisual;

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
        timeSinceLastShield = 0f;

        // Create circular shield
        shieldVisual = new GameObject("ShieldVisual");
        shieldVisual.transform.position = (Vector2)transform.position + shieldOffset;
        shieldVisual.transform.localScale = Vector3.one * shieldRadius * 2f; // Double for sprite radius

        // Add sprite renderer with blue circle
        SpriteRenderer renderer = shieldVisual.AddComponent<SpriteRenderer>();
        renderer.sprite = CreateCircleSprite();
        renderer.color = new Color(0.2f, 0.6f, 1f, shieldOpacity); // Light blue
        renderer.sortingOrder = 10; // Make sure it renders above player

        // Add trigger collider
        CircleCollider2D collider = shieldVisual.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 0.5f; // Matches sprite size

        // Add collision handler
        shieldVisual.AddComponent<ShieldCollisionHandlerP2>().Initialize(this);

        yield return new WaitForSeconds(shieldDuration);

        Destroy(shieldVisual);
        isShieldActive = false;
    }

    private Sprite CreateCircleSprite()
    {
        // Create a simple circle texture
        Texture2D tex = new Texture2D(128, 128);
        Color[] pixels = tex.GetPixels();
        
        Vector2 center = new Vector2(tex.width / 2, tex.height / 2);
        float radius = tex.width / 2;

        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), center);
                pixels[y * tex.width + x] = dist <= radius ? Color.white : Color.clear;
            }
        }
        
        tex.SetPixels(pixels);
        tex.Apply();
        
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
    }

    public void HandleCollision(GameObject other)
    {
        if (other.CompareTag("Projectile"))
        {
            Destroy(other);
            Debug.Log("Projectile blocked by shield!");
        }
    }
}

public class ShieldCollisionHandlerP2 : MonoBehaviour
{
    private ShieldControllerP2 controller;

    public void Initialize(ShieldControllerP2 ctrl)
    {
        controller = ctrl;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        controller?.HandleCollision(other.gameObject);
    }
}