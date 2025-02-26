using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject powerUpPrefab;
    public Transform[] spawnPoints; // Assign these in the Inspector
    public float spawnInterval = 5f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnPowerUp), spawnInterval, spawnInterval);
    }

    private void SpawnPowerUp()
    {
        if (spawnPoints.Length == 0) return; // Ensure there are spawn points

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(powerUpPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
    }
}
