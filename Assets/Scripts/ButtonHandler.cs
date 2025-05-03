using UnityEngine;

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