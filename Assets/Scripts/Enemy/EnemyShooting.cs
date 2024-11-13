using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public Transform shootPoint;

    private void Start()
    {
        StartCoroutine(ShootAtIntervals());
    }

    IEnumerator ShootAtIntervals()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        }
    }
}
