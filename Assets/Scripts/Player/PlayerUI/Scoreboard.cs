using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField]private GameObject ScoreboardUI;
    void Start()
    {
        ScoreboardUI.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)){
            ScoreboardUI.SetActive(true);
        }else if (Input.GetKeyUp(KeyCode.Tab)){
            ScoreboardUI.SetActive(false);
        }
        
    }
}
