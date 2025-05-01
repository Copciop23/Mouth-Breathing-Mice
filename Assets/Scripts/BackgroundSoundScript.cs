using UnityEngine;

public class BackgroundSoundScript : MonoBehaviour
{
    void Start()
    {

        AudioManager.Instance.PlaySound("BackgroundMusic", AudioManager.AudioType.Music);
    }

}
