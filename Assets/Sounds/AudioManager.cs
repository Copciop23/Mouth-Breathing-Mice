using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;

    private Dictionary<string, AudioClip> sfxLibrary = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> musicLibrary = new Dictionary<string, AudioClip>();

    private AudioSource musicSource;
    private List<AudioSource> sfxSources = new List<AudioSource>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;

        for (int i = 0; i < 5; i++)
        {
            AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSources.Add(sfxSource);
        }

        LoadSounds();
    }
    private void LoadSounds()
    {
        Transform sfxChild = transform.Find("SFX");
        if (sfxChild != null)
        {
            AudioSource[] sfxAudioSources = sfxChild.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource source in sfxAudioSources)
            {
                if (!sfxLibrary.ContainsKey(source.gameObject.name))
                {
                    sfxLibrary.Add(source.gameObject.name, source.clip);
                }
                else
                {
                    Debug.LogWarning($"SFX with name '{source.gameObject.name}' already exists.");
                }
            }
        }
        else
        {
            Debug.LogWarning("SFX child object not found.");
        }

        Transform musicChild = transform.Find("Music");
        if (musicChild != null)
        {
            AudioSource[] musicAudioSources = musicChild.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource source in musicAudioSources)
            {
                if (!musicLibrary.ContainsKey(source.gameObject.name))
                {
                    musicLibrary.Add(source.gameObject.name, source.clip);
                }
                else
                {
                    Debug.LogWarning($"Music with name '{source.gameObject.name}' already exists.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Music child object not found.");
        }
    }

    public void PlaySound(string soundName, AudioType type)
    {
        switch (type)
        {
            case AudioType.SFX:
                if (sfxLibrary.TryGetValue(soundName, out AudioClip sfxClip))
                {
                    PlaySFX(sfxClip);
                }
                else
                {
                    Debug.LogWarning($"SFX '{soundName}' not found.");
                }
                break;

            case AudioType.Music:
                if (musicLibrary.TryGetValue(soundName, out AudioClip musicClip))
                {
                    PlayMusic(musicClip);
                }
                else
                {
                    Debug.LogWarning($"Music '{soundName}' not found.");
                }
                break;

            default:
                Debug.LogWarning($"Unknown audio type: {type}");
                break;
        }
    }

    private void PlaySFX(AudioClip clip)
    {
        AudioSource availableSource = sfxSources.Find(source => !source.isPlaying);
        if (availableSource != null)
        {
            availableSource.clip = clip;
            availableSource.volume = masterVolume * sfxVolume;
            availableSource.Play();
        }
        else
        {
            Debug.LogWarning("No available AudioSource for SFX.");
        }
    }
    private void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.volume = masterVolume * musicVolume;
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void UpdateVolumes()
    {
        musicSource.volume = masterVolume * musicVolume;

        foreach (var source in sfxSources)
        {
            source.volume = masterVolume * sfxVolume;
        }
    }

    public enum AudioType
    {
        SFX,
        Music
    }
}