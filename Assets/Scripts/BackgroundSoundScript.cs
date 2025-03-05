using UnityEngine;

public class BackgroundSoundScript : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlaySound("BackgroundSong", AudioManager.AudioType.Music);
    }

}
