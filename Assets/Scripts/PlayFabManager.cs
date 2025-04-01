using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{
    public InputField nameInput;
    public int score;
    public Text messageText;
    public InputField emailInput;
    public InputField passwordInput;

    // Register a new PlayFab account
    public void RegisterButton()
    {
        if (passwordInput.text.Length < 6)
        {
            messageText.text = "Password Too Short!";
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered and Logged In!";
        Debug.Log("Registration Successful!");
    }

    // Log in with an existing PlayFab account
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
        messageText.text = "Logged In!";
        Debug.Log("Login Successful!");

        // Load the main scene after login
        SceneManager.LoadScene("main");

        // Set the player's display name if available
        if (result.InfoResultPayload.PlayerProfile != null)
            PlayerData.playerName = result.InfoResultPayload.PlayerProfile.DisplayName;

        // Default name if display name is not set
        if (string.IsNullOrEmpty(PlayerData.playerName))
            PlayerData.playerName = "Player";

        // Fetch leaderboard data
        GetLeaderboard();
    }

    // Reset password using email
    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "794EE" // Replace with your Title ID
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    private void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password Reset Email Sent!";
    }

    // Update the player's display name
    public void SubmitNameButton()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Display Name Updated!");
    }

    // Send player statistics to the leaderboard
    public void SendLeaderboard(string statisticName, int value)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = statisticName, // Corrected field name
                    Value = value
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdateSuccess, OnError);
    }

    private void OnLeaderboardUpdateSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Leaderboard Updated Successfully!");
    }

    // Retrieve leaderboard data
    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "KillsInTotal", // Replace with your statistic name
            StartPosition = 0,
            MaxResultsCount = 10 // Adjust the number of results as needed
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardSuccess, OnLeaderboardError);
    }

    private void OnLeaderboardSuccess(GetLeaderboardResult result)
    {
        Debug.Log("Leaderboard Retrieved Successfully!");
        foreach (var entry in result.Leaderboard)
        {
            Debug.Log($"Position: {entry.Position}, Player: {entry.DisplayName}, Score: {entry.StatValue}");
        }
    }

    private void OnLeaderboardError(PlayFabError error)
    {
        Debug.LogError($"Error Retrieving Leaderboard: {error.GenerateErrorReport()}");
    }

    // Handle errors
    private void OnError(PlayFabError error)
    {
        Debug.LogError($"PlayFab Error: {error.GenerateErrorReport()}");
        messageText.text = $"Error: {error.GenerateErrorReport()}";
    }
}