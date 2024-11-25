using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public GameObject shieldPrefab; // Shield prefab
    private GameObject activeShield; // Reference to the active shield

    void Update()
    {
        // If 'H' key is pressed, spawn shield
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (activeShield == null)
            {
                activeShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
            }
        }

        // If 'H' key is released, destroy shield
        if (Input.GetKeyUp(KeyCode.H))
        {
            if (activeShield != null)
            {
                Destroy(activeShield);
            }
        }

        // Update the shield's position to follow the mouse
        if (activeShield != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Ensure 2D positioning
            activeShield.transform.position = transform.position + (mousePosition - transform.position).normalized;
        }
    }
}