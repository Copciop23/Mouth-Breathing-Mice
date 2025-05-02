using UnityEngine;
using System.Collections;

public class PlayerPunch : MonoBehaviour
{
    public int punchDamage = 10;
    public Vector2 punchBoxSize = new Vector2(1.5f, 1f);
    public float punchOffset = 0.75f;
    public LayerMask punchableLayer;
    private KeyCode punchKey = KeyCode.Q;

    private bool isPunching = false;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerStats playerStats;
    private Movement movement;
    private bool isFacingRight = true;

    void Start()
    {
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
            isFacingRight = false;
        else if (Input.GetKeyDown(KeyCode.D))
            isFacingRight = true;

        if (Input.GetKeyDown(punchKey) && !isPunching)
        {
            StartCoroutine(PunchAction());
        }
    }

    private IEnumerator PunchAction()
    {
        isPunching = true;
        movement.enabled = false;

        animator?.CrossFade("punch-albino", 0f);
        yield return new WaitForSeconds(0.1f);

        float direction = isFacingRight ? 1f : -1f;
        Vector2 boxCenter = (Vector2)transform.position + Vector2.right * direction * punchOffset;

        Collider2D[] hitObjects = Physics2D.OverlapBoxAll(boxCenter, punchBoxSize, 0f, punchableLayer);

        foreach (Collider2D hitObject in hitObjects)
        {
            if (hitObject.CompareTag("Player"))
            {
                PlayerStats otherPlayer = hitObject.GetComponent<PlayerStats>();
                if (otherPlayer != null && otherPlayer != playerStats)
                {
                    otherPlayer.Health.TakeDamage(punchDamage);
                    AudioManager.Instance.PlaySound("punch_hit", AudioManager.AudioType.SFX);
                }
            }

            if (hitObject.TryGetComponent(out EnemyHurt enemy))
            {
                enemy.TakeDamage(punchDamage, gameObject);
                AudioManager.Instance.PlaySound("punch_hit", AudioManager.AudioType.SFX);
            }
        }

        yield return new WaitForSeconds(0.2f);
        movement.enabled = true;
        isPunching = false;
    }

    private void OnDrawGizmosSelected()
    {
        float direction = isFacingRight ? 1f : -1f;
        Vector3 boxCenter = transform.position + Vector3.right * direction * punchOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCenter, punchBoxSize);
    }
}