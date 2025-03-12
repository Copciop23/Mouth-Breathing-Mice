using UnityEngine;

public class EnemyShooting : MonoBehaviour {
    public GameObject bulletPrefab;
    public Transform firePoint; // Assign a child GameObject as fire point
    public float shootRange = 6f;
    public float bulletSpeed = 10f;
    public float predictionFactor = 0.5f;
    public float fireRate = 1.5f; // Time between shots
    private Transform player;
    private float nextFireTime = 0f;
    private Rigidbody2D playerRb; 

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Make sure the player has the tag "Player"
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= shootRange && Time.time >= nextFireTime) {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot() {
        Vector2 playerVelocity = playerRb.linearVelocity;
        float predictedOffset = playerVelocity.x * predictionFactor;

        Vector2 predictedPosition = new Vector2(player.position.x + predictedOffset, player.position.y);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * bulletSpeed, 0);
    }
}