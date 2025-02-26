using System.Collections;
using UnityEngine;

public class Punch : MonoBehaviour
{
    [Header("Punch Settings")]
    public int punchDamage = 10;
    public float punchRange = 1f;
    public Transform punchPoint;
    public LayerMask punchableLayer;

    private bool isPunching = false;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isPunching)
        {
            StartCoroutine(PunchAction());
        }
    }

    private IEnumerator PunchAction()
    {
        isPunching = true;

        AudioManager.Instance.PlaySound("punch", AudioManager.AudioType.SFX);
        animator.CrossFade("punch", 0, 0);

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(punchPoint.position, punchRange, punchableLayer);
        foreach (Collider2D hitObject in hitObjects)
        {

            if (hitObject.TryGetComponent(out EnemyHurt enemy))
            {
                enemy.TakeDamage(punchDamage, gameObject);
                AudioManager.Instance.PlaySound("punch_hit", AudioManager.AudioType.SFX);
            }
        }

        yield return new WaitForSeconds(0.3f);
        isPunching = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (punchPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(punchPoint.position, punchRange);
        }
    }
}
