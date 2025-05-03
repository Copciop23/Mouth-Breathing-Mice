using UnityEngine;

public class BackgroundSoundScript : MonoBehaviour
{
    void Start()
    {

        AudioManager.Instance.PlaySound("MainMenuSong", AudioManager.AudioType.Music);
    }

}
