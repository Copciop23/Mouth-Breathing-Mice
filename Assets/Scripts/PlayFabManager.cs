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
    // UI Elements
    public Text usernametttt;                 // Displays logged-in username
    public InputField usernameInput;          // For registration username input
    public Text leaderboardTitle;             // Title of the leaderboard display
    public GameObject rowPrefab;              // Prefab for leaderboard rows
    public Transform rowsParent;              // Parent object for leaderboard rows
    public InputField nameInput;              // For display name changes
    public int score;                        // Player score (unused in current implementation)
    public Text messageText;                 // Displays system messages/errors
    public InputField emailInput;            // Email input for login/registration
    public InputField passwordInput;         // Password input for login/registration

    /// <summary>
    /// Handles player registration with PlayFab
    /// </summary>
    public void RegisterButton()
    {
        // Validate password length
        if (passwordInput.text.Length < 6)
        {
            messageText.text = "Password Too short!";
            return;
        }

        // Create registration request
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            Username = usernameInput.text,
            RequireBothUsernameAndEmail = true,
            DisplayName = usernameInput.text
        };

        // Send registration request to PlayFab
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    /// <summary>
    /// Callback for successful registration
    /// </summary>
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        // Fetch player data after registration
        GetPlayerStats();
        GetLeaderboard("KillsInTotal", "Kills");

        // Handle username display
        if (result.Username != null)
        {
            Debug.Log("Has a profile");
            Debug.Log(result.Username);
            PlayerData.playerName = result.Username;
            usernametttt.text = "Logged in as: " + result.Username;
        }
        else
        {
            Debug.Log("Hasn't a profile");
            PlayerData.playerName = "ExampleName";
            usernametttt.text = "Logged in as: Guest";
        }
    }

    /// <summary>
    /// Handles player login with email/password
    /// </summary>
    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true  // Request player profile data
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    /// <summary>
    /// Callback for successful login
    /// </summary>
    private void OnLoginSuccess(LoginResult result)
    {
        // Clear sensitive inputs
        emailInput.text = "";
        passwordInput.text = "";
        messageText.text = "Logged in";
        Debug.Log("Successful Login");

        // Fetch player data
        GetPlayerStats();
        GetLeaderboard("KillsInTotal", "Kills");

        // Handle profile display
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            Debug.Log("Has a profile");
            Debug.Log(result.InfoResultPayload.PlayerProfile.DisplayName);
            PlayerData.playerName = result.InfoResultPayload.PlayerProfile.DisplayName;
            usernametttt.text = "Logged in as: " + result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        else
        {
            Debug.Log("Hasn't a profile");
            PlayerData.playerName = "ExampleName";
            usernametttt.text = "Logged in as: Guest";
        }
    }

    /// <summary>
    /// Handles password reset requests
    /// </summary>
    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "794EE"  // Your PlayFab title ID
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    /// <summary>
    /// Updates player's display name
    /// </summary>
    public void SubmitNameButton()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    /// <summary>
    /// Callback for successful display name update
    /// </summary>
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated Display name!");
    }

    /// <summary>
    /// Callback for password reset request
    /// </summary>
    private void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password Reset Mail Sent";
    }

    /// <summary>
    /// Fetches player statistics from PlayFab
    /// </summary>
    void GetPlayerStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
        result => {
            Debug.Log("Got player statistics:");
            foreach (var stat in result.Statistics)
            {
                Debug.Log($"Statistic: {stat.StatisticName} = {stat.Value}");
                // Update local player data
                if (stat.StatisticName == "WinsInTotal")
                {
                    PlayerData.winssss = stat.Value;
                    Debug.Log(PlayerData.winssss);
                }
                if (stat.StatisticName == "KillsInTotal")
                {
                    PlayerData.killssss = stat.Value;
                    Debug.Log(PlayerData.killssss);
                }
                if (stat.StatisticName == "DeathsInTotal")
                {
                    PlayerData.deathssss = stat.Value;
                    Debug.Log(PlayerData.deathssss);
                }
            }
        },
        error => {
            Debug.LogError("Error getting statistics: " + error.GenerateErrorReport());
        });
    }

    /// <summary>
    /// Handles custom ID login (device-specific)
    /// </summary>
    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    /// <summary>
    /// Generic success callback
    /// </summary>
    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful Login/account created!");
    }

    /// <summary>
    /// Generic error callback
    /// </summary>
    void OnError(PlayFabError error)
    {
        Debug.Log("Error While creating/Logging in");
        usernametttt.text = "Failed to Login";
        Debug.Log(error.GenerateErrorReport());
    }

    /// <summary>
    /// Updates leaderboard statistics
    /// </summary>
    public static void SendLeaderboard(string StatisticName, int value)
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
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

    /// <summary>
    /// Retrieves and displays leaderboard data
    /// </summary>
    public void GetLeaderboard(String StatisticNamee, string titlee)
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
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

    /// <summary>
    /// Callback for successful leaderboard fetch
    /// </summary>
    private void OnLeaderboardSuccess(GetLeaderboardResult result)
    {
        Debug.Log("Leaderboard results received");
        Debug.Log(result.Leaderboard.Count);

        // Clear existing leaderboard rows
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        // Create new rows for each leaderboard entry
        foreach (var entry in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = entry.Profile.DisplayName;
            texts[1].text = entry.StatValue.ToString();
            Debug.Log($"Position: {entry.Position}, Character: {entry.PlayFabId}, Score: {entry.StatValue}");
        }
    }

    /// <summary>
    /// Callback for failed leaderboard fetch
    /// </summary>
    private void OnLeaderboardError(PlayFabError error)
    {
        Debug.LogError($"Error retrieving leaderboard: {error.GenerateErrorReport()}");
    }

    // Convenience methods for specific leaderboards
    public void killLeaderboard() => GetLeaderboard("KillsInTotal", "Kills");
    public void winsleaderboard() => GetLeaderboard("WinsInTotal", "Wins");
    public void deathleaderboard() => GetLeaderboard("DeathsInTotal", "Deaths");

    /// <summary>
    /// Initialization
    /// </summary>
    void Start()
    {
        Debug.Log("This script is attached to: " + gameObject.name);
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            StartCoroutine(StartWithDelay());
        }
        PlayFabSettings.staticSettings.TitleId = "794EE";  // Set your PlayFab title ID
    }

    /// <summary>
    /// Delayed initialization coroutine
    /// </summary>
    public IEnumerator StartWithDelay()
    {
        yield return new WaitForSeconds(1);
        loadscener();
    }

    /// <summary>
    /// Loads initial scene data after login
    /// </summary>
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