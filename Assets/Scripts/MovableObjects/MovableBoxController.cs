using UnityEngine;

public class MovableBoxController : MonoBehaviour
{

    public float pushForce = 4f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector2 direction = collision.transform.position - transform.position;
            rb.AddForce(-direction.normalized * pushForce);
        }
    }
}
