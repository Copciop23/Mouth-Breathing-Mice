using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;


public class Usernames : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI usernameHolder;
    string playerName = PlayerData.playerName;


    private void Start()
    {
        if (playerName != null)
        {
            usernameHolder.text = playerName;
        }
    }
}

