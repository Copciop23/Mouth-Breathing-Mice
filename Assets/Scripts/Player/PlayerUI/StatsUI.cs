using UnityEngine;
using TMPro;
public class StatsUI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TextMeshProUGUI playerHealthText;
    void Start()
    {
        playerHealthText.text="<color=#AA0000>HP: <color=#FF5555>"+playerStats.playerHealth+"</color>";
    }

    // Update is called once per frame
    void Update()
    {
       playerHealthText.text="<color=#AA0000>HP: <color=#FF5555>"+playerStats.playerHealth+"</color>"; 
    }
}
