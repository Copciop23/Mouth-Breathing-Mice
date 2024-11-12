using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;


private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        doDamage();
    }
}

        public void doDamage(){
        if(playerStats.playerHealth==null)return;
        if(playerStats.playerHealth>0){
            playerStats.playerHealth -= 2;
        }
    }

}
