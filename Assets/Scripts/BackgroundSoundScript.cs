using UnityEngine;

public class BackgroundSoundScript : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundAudio;
    void Start()
    {
        backgroundAudio.Play();
    }

}
