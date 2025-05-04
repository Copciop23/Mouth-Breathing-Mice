using UnityEngine;

/// <summary>
/// Manages the user interface for login and registration, and transitions to the game mode menu.
/// </summary>
public class LoginRegisterManager : MonoBehaviour
{
    // UI Panels for login and registration
    public GameObject loginPanel;
    public GameObject registerPanel;

    // Canvas references for the login/register screen and game mode menu
    public GameObject menuGamemodeCanvas;
    public GameObject loginRegisterCanvas;

    /// <summary>
    /// Called when the script is first run. Displays the login panel by default.
    /// </summary>
    void Start()
    {
        ShowLoginPanel(); // Ensure the login panel is shown at the start
    }

    /// <summary>
    /// Displays the login panel and hides the registration panel.
    /// </summary>
    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true); // Activate the login panel
        registerPanel.SetActive(false); // Deactivate the registration panel
    }

    /// <summary>
    /// Displays the registration panel and hides the login panel.
    /// </summary>
    public void ShowRegisterPanel()
    {
        loginPanel.SetActive(false); // Deactivate the login panel
        registerPanel.SetActive(true); // Activate the registration panel
    }

    /// <summary>
    /// Switches to the game mode menu, hiding the login/register canvas.
    /// </summary>
    public void LoginToMenu()
    {
        menuGamemodeCanvas.SetActive(true); // Show the game mode menu
        loginRegisterCanvas.SetActive(false); // Hide the login/register canvas
    }
}
