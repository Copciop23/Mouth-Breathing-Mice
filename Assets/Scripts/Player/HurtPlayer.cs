using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;


private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        doDamage();
    }
}

        public void doDamage(){
        if (playerStats.playerHealth == 0) return;
        if(playerStats.playerHealth>0){
            playerStats.playerHealth -= 2;
        }
    }

}
