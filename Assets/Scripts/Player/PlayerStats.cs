using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    public int playerHealth;
    public Slider slider;

    void Start()
    {
        playerHealth = 100;
        slider.value=100;
    }

     private void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.CompareTag("Spike")) {
                doDamage(20);
        }
     }

     public void doDamage(int damage){
        if (playerHealth <= 0){
            Die();
        }
        if(playerHealth>0){
            playerHealth -= damage;
        }
        
    }
    private void Die() {
        Debug.Log("Player died. :(");
        playerHealth=100;

    }

}

