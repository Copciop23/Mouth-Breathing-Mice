using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class UpdatePlayerStatsExample : MonoBehaviour
{
    void Start()
    {
        // Log in the player (replace with your own login logic)
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier, // Use device ID for simplicity
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login successful!");
        // Update player statistics after login
        UpdatePlayerStatistics();
    }

    void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Login failed: " + error.GenerateErrorReport());
    }

    void UpdatePlayerStatistics()
    {
        // Create the request object
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Score", // Statistic name
                    Value = 1000              // New value for the statistic
                },
                new StatisticUpdate
                {
                    StatisticName = "Level", // Statistic name
                    Value = 5               // New value for the statistic
                }
            }
        };

        // Make the API call
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdateStatsSuccess, OnUpdateStatsFailure);
    }

    void OnUpdateStatsSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Player statistics updated successfully!");
    }

    void OnUpdateStatsFailure(PlayFabError error)
    {
        Debug.LogError("Failed to update player statistics: " + error.GenerateErrorReport());
    }
}