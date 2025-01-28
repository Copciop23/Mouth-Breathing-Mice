using UnityEngine;
using TMPro;
public class StatsUI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    void Start()
    {
        playerScoreText.text= "<color=#81D8D0>KILLS: <color=#5555FF>" + playerStats.getkills+"</color>";
    }

    void Update()
    {
       playerScoreText.text= "<color=#81D8D0>Health: <color=#5555FF>"+ playerStats.playerHealth + "\n<color=#81D8D0>KILLS: <color=#5555FF>" + playerStats.getkills+"</color>"; 
    }
}
