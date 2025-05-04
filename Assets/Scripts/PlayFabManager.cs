using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using NUnit.Framework;
using System.Collections.Generic;
using PlayFab.ProgressionModels;
using PlayFab.SharedModels;
using PlayFab.ProgressionModels;
using UnityEngine.SocialPlatforms.Impl;
using JetBrains.Annotations;
using TMPro;
using System.Collections;

public class PlayFabManager : MonoBehaviour
{
    public Text usernametttt;
    public InputField usernameInput;
    public Text leaderboardTitle;
    public GameObject rowPrefab;
    public Transform rowsParent;
    public InputField nameInput;
    public int score;
    public Text messageText;
    public InputField emailInput;
    public InputField passwordInput;
    public void RegisterButton()
    {
        if (passwordInput.text.Length < 6)
        {
            messageText.text = "Password Too short!";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            Username = usernameInput.text,
            RequireBothUsernameAndEmail = true,
            DisplayName = usernameInput.text
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        GetPlayerStats();
        GetLeaderboard("KillsInTotal", "Kills");
        if (result.Username != null)
        {
            Debug.Log("Has a profile");
            Debug.Log(result.Username);
            PlayerData.playerName = result.Username;
            usernametttt.text = "Logged in as: " + result.Username;
        }
        if (result.Username == null)
        {
            Debug.Log("Hasnt a profile");
            PlayerData.playerName = "ExampleName";
            usernametttt.text = "Logged in as: Guest";
        }
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }

        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        emailInput.text = "";
        passwordInput.text = "";
        messageText.text = "Logged in";
        Debug.Log("Succesful Login");
        GetPlayerStats();
        GetLeaderboard("KillsInTotal", "Kills");
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            Debug.Log("Has a profile");
            Debug.Log(result.InfoResultPayload.PlayerProfile.DisplayName);
            PlayerData.playerName = result.InfoResultPayload.PlayerProfile.DisplayName;
            usernametttt.text = "Logged in as: " + result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        if (result.InfoResultPayload.PlayerProfile == null)
        {
            Debug.Log("Hasnt a profile");
            PlayerData.playerName = "ExampleName";
            usernametttt.text = "Logged in as: Guest";
        }
    }
    //55
    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "794EE"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);

    }
    public void SubmitNameButton()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);

    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated Display name!");
    }

    private void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password Reset Mail Sent";
    }










    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    // Update is called once per frame

    void GetPlayerStats() {
    PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), 
    result => {
        Debug.Log("Got player statistics:");
        foreach (var stat in result.Statistics) {
            Debug.Log($"Statistic: {stat.StatisticName} = {stat.Value}");
            if (stat.StatisticName == "WinsInTotal") {
                PlayerData.winssss = stat.Value;
                Debug.Log(PlayerData.winssss);
            }
            if (stat.StatisticName == "KillsInTotal") {
                PlayerData.killssss = stat.Value;
                Debug.Log(PlayerData.killssss);
            }
            if (stat.StatisticName == "DeathsInTotal") {
                PlayerData.deathssss = stat.Value;
                Debug.Log(PlayerData.deathssss);
            }
        }
    }, 
    error => {
        Debug.LogError("Error getting statistics: " + error.GenerateErrorReport());
    });
}
    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);

    }
    void OnSuccess(LoginResult result)
    {
        Debug.Log("Succesful Login/account created!");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error While creating/Logging in");
        usernametttt.text = "Failed to Login";
        Debug.Log(error.GenerateErrorReport());
    }
    public static void SendLeaderboard(string StatisticName, int value)
    {
        if (PlayFabClientAPI.IsClientLoggedIn()){
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<PlayFab.ClientModels.StatisticUpdate> {
            new PlayFab.ClientModels.StatisticUpdate {
                StatisticName = StatisticName,
                Value = value
            }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request,
        result => {
            Debug.Log($"Successfully updated stat");
        },
        error => {
            Debug.LogError("Error updating stat: " + error.GenerateErrorReport());
        });
        }
    }

    public void GetLeaderboard(String StatisticNamee, string titlee)
    {
        if (PlayFabClientAPI.IsClientLoggedIn()) {
        leaderboardTitle.text = titlee;
        var request = new GetLeaderboardRequest
        {
            StatisticName = StatisticNamee,
            StartPosition = 0,
            MaxResultsCount = 5,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowDisplayName = true
            }


        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardSuccess, OnLeaderboardError);
        }
    }
    private void OnLeaderboardSuccess(GetLeaderboardResult result)
    {
        Debug.Log("Giving u player nubmer 1");
        Debug.Log(result.Leaderboard.Count);
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }


        foreach (var entry in result.Leaderboard)
        {

            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = entry.Profile.DisplayName;
            texts[1].text = entry.StatValue.ToString();
            Debug.Log("Giving u player nubmer 1");
            Debug.Log($"Position: {entry.Position}, Character: {entry.PlayFabId}, Score: {entry.StatValue}");
        }
    }
    private void OnLeaderboardError(PlayFabError error)
    {
        Debug.LogError($"Error retrieving leaderboard: {error.GenerateErrorReport()}");
    }
    public void killLeaderboard()
    {
        GetLeaderboard("KillsInTotal", "Kills");
    }
    public void winsleaderboard()
    {
        GetLeaderboard("WinsInTotal", "Wins");
    }
    public void deathleaderboard()
    {
        GetLeaderboard("DeathsInTotal", "Deaths");
    }

    private void OnSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfully updated player statistics.");
    }
    void Start()
    {
        Debug.Log("This script is attached to: " + gameObject.name);
        if (PlayFabClientAPI.IsClientLoggedIn())
    {
        StartCoroutine(StartWithDelay());
    }
        PlayFabSettings.staticSettings.TitleId = "794EE";
        
    }
    public IEnumerator StartWithDelay()
    {
        yield return new WaitForSeconds(1);
        loadscener();
    }
    public void loadscener()
    {
        if (usernametttt == null || leaderboardTitle == null || rowsParent == null)
{
        Debug.LogError("UI references not set. Did you forget to assign them in the scene?");
}
        usernametttt.text = "Logged in as: " + PlayerData.playerName;
        GetPlayerStats();
        killLeaderboard();
    }


}





