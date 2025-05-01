using UnityEngine;

public class LoginRegisterManager : MonoBehaviour
{
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject menuGamemodeCanvas;
    public GameObject loginRegisterCanvas;

    void Start()
    {
        ShowLoginPanel();
    }

    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }

    public void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void LoginToMenu() {
        menuGamemodeCanvas.SetActive(true);
        loginRegisterCanvas.SetActive(false);
    }
}