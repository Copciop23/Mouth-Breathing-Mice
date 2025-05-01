using UnityEngine;
using System.Collections;

public class PlayerPunch : MonoBehaviour
{
    public int punchDamage = 10;
    public float punchRange = 2f; // Ensure this is large enough for Player 1
    public Transform punchPoint;
    public LayerMask punchableLayer; // Make sure Player and Enemy layers are included
    private KeyCode punchKey = KeyCode.Z;

    private bool isPunching = false;
    [SerializeField] private Animator animator;
    private Movement movement;
    [SerializeField] private PlayerStats playerStats;  // Reference to PlayerStats to exclude the punching player

    void Start()
    {
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        // Detect punch input (Player 1's punch key)
        if (Input.GetKeyDown(punchKey) && !isPunching)
        {
            StartCoroutine(PunchAction());
        }
    }

    private IEnumerator PunchAction()
    {
        isPunching = true;

        // Disable movement while punching (optional)
        movement.enabled = false; // Temporarily disable movement

        // Trigger punch animation (ensure "punch" is the correct animation name)
        if (animator != null)
        {
            animator.CrossFade("punch", 0f);  // Ensure this is the correct animation state
        }
        else
        {
            Debug.LogWarning("Animator is not assigned.");
        }

        // Debug log to confirm punch action is happening
        Debug.Log("Punch action started");

        // Detect all objects within the punch range
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(punchPoint.position, punchRange, punchableLayer);

        if (hitObjects.Length == 0)
        {
            Debug.Log("No objects hit within punch range.");
        }

        foreach (Collider2D hitObject in hitObjects)
        {
            Debug.Log($"Hit Object: {hitObject.gameObject.name}");

            // Detect if it's another player (and not the current player)
            if (hitObject.CompareTag("Player"))
            {
                PlayerStats otherPlayerStats = hitObject.GetComponent<PlayerStats>();

                if (otherPlayerStats != playerStats) // Make sure we don't hit the player who's punching
                {
                    Debug.Log($"Punch hit player: {hitObject.gameObject.name}");
                    otherPlayerStats.Health.TakeDamage(punchDamage);
                    AudioManager.Instance.PlaySound("punch_hit", AudioManager.AudioType.SFX);
                }
                else
                {
                    Debug.Log("Punch hit self, skipping damage.");
                }
            }

            // Check if the object is an enemy
            if (hitObject.TryGetComponent(out EnemyHurt enemy))
            {
                Debug.Log($"Punch hit enemy: {hitObject.gameObject.name}");
                enemy.TakeDamage(punchDamage, gameObject);
                AudioManager.Instance.PlaySound("punch_hit", AudioManager.AudioType.SFX);
            }
        }

        // Wait for the animation to finish (assuming punch lasts for 0.3 seconds)
        yield return new WaitForSeconds(0.3f);

        // Re-enable movement after punching
        movement.enabled = true;

        isPunching = false;
    }

    // Gizmo to visualize the punch range in the Unity editor
    private void OnDrawGizmosSelected()
    {
        if (punchPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(punchPoint.position, punchRange);  // Visualize the larger punch range
        }
    }
}