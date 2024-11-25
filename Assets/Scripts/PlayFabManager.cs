using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{
   public Text messageText;
   public InputField emailInput;
   public InputField passwordInput;
   public void RegisterButton() {
    if (passwordInput.text.Length < 6) {
        messageText.text = "Password Too short!";
        return;
    }
        var request = new RegisterPlayFabUserRequest {
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
   }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered";
    }

   public void LoginButton() {
    var request = new LoginWithEmailAddressRequest {
        Email = emailInput.text,
        Password = passwordInput.text
    };
    PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
   }

    private void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Logged in";
        Debug.Log("Succesful Login");
        SceneManager.LoadScene("main");
    }

    public void ResetPasswordButton() {
        var request = new SendAccountRecoveryEmailRequest {
            Email = emailInput.text,
            TitleId = "794EE"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
   }

    private void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password Reset Mail Sent";
    }










    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Login()
    {
        var request = new LoginWithCustomIDRequest {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }
    void OnSuccess(LoginResult result) {
        Debug.Log("Succesful Login/account created!");
    }

    void OnError(PlayFabError error) {
        Debug.Log("Error While creating/Logging in");
        Debug.Log(error.GenerateErrorReport());
    }
}
