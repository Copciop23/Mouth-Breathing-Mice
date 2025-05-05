using UnityEngine;

//button handler script for triggering music upon clicking buttons with this handler
public class ButtonHandler : MonoBehaviour
{
    public void SwitchMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SwitchToBackgroundMusic();
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found. Ensure it exists and is marked DontDestroyOnLoad.");
        }
    }
}