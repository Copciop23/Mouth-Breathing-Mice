using UnityEngine;

public class HawkTuah : MonoBehaviour
{
    public GameObject spitPrefab;
    public Transform firePoint;
    public float spitSpeed = 5f;

    private bool isFacingRight = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            isFacingRight = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            isFacingRight = true;
        }

        firePoint.localPosition = isFacingRight ? new Vector2((float)0.1, 0) : new Vector2((float)-0.1, 0);

        // Shoot the spit when 'G' is pressed
        if (Input.GetKeyDown(KeyCode.G))
        {
            ShootSpit();
        }
    }

    void ShootSpit()
{
    GameObject spit = Instantiate(spitPrefab, firePoint.position, firePoint.rotation);

    SpitProjectileBehaviour projectileBehaviour = spit.GetComponent<SpitProjectileBehaviour>();
    if (projectileBehaviour != null)
    {
        projectileBehaviour.isFacingRight = isFacingRight;
    }
}
}
