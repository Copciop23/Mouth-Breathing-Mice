using UnityEngine;

public class EnemyShooting : MonoBehaviour {
    public GameObject bulletPrefab;
    public Transform firePoint; // Assign a child GameObject as fire point
    public float shootRange = 6f;
    public float bulletSpeed = 10f;
    public float fireRate = 1.5f; // Time between shots

    private Transform player;
    private float nextFireTime = 0f;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Make sure the player has the tag "Player"
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
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.velocity = new Vector2(direction * bulletSpeed, 0);
    }
}
