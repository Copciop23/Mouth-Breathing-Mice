using UnityEngine;
using UnityEngine.SceneManagement;

// Script for switching scenes

public class SceneSwitchManager : MonoBehaviour
{
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
