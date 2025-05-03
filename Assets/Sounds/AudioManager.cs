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
            masterVolume = PlayerPrefs.GetFloat("MasterVolume", 100f) / 100;
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 100f) / 100;
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 100f) / 100;

            Debug.Log($"Loaded Volumes - Master: {masterVolume}, SFX: {sfxVolume}, Music: {musicVolume}");

            DontDestroyOnLoad(gameObject);

            // Initialize audio sources
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;

            for (int i = 0; i < 5; i++)
            {
                AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSources.Add(sfxSource);
            }

            LoadSounds();

            // Update volumes after initializing everything
            UpdateVolumes();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void LoadSounds()
    {
        Transform musicChild = transform.Find("Music");
        if (musicChild != null)
        {
            AudioSource[] musicAudioSources = musicChild.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource source in musicAudioSources)
            {
                if (!musicLibrary.ContainsKey(source.gameObject.name))
                {
                    musicLibrary.Add(source.gameObject.name, source.clip);
                    Debug.Log($"Added music clip '{source.gameObject.name}' to library.");
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

        Transform sfxChild = transform.Find("SFX");
        if (sfxChild != null)
        {
            AudioSource[] sfxAudioSources = sfxChild.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource source in sfxAudioSources)
            {
                if (!sfxLibrary.ContainsKey(source.gameObject.name))
                {
                    sfxLibrary.Add(source.gameObject.name, source.clip);
                    Debug.Log($"Added SFX clip '{source.gameObject.name}' to library.");
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
    }

    public void PlaySound(string soundName, AudioType type)
    {
        Debug.Log($"Attempting to play sound: {soundName} of type: {type}");
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
        Debug.Log($"Playing music with volume: {musicSource.volume}");
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void UpdateVolumes()
    {
        Debug.Log($"Updating Volumes - Master: {masterVolume}, SFX: {sfxVolume}, Music: {musicVolume}");
        musicSource.volume = masterVolume * musicVolume;

        foreach (var source in sfxSources)
        {
            source.volume = masterVolume * sfxVolume;
        }
    }

    public void SwitchToBackgroundMusic()
    {
        Debug.Log("Switching from MainMenuSong to BackgroundMusic.");

        if (musicLibrary.TryGetValue("MainMenuSong", out AudioClip mainMenuSong))
        {
            if (musicSource.clip == mainMenuSong && musicSource.isPlaying)
            {
                musicSource.Stop();
            }
        }
        else
        {
            Debug.LogWarning("MainMenuSong not found in the library.");
        }

        if (musicLibrary.TryGetValue("BackgroundMusic", out AudioClip backgroundMusic))
        {
            if (musicSource.clip != backgroundMusic)
            {
                musicSource.clip = backgroundMusic;
                musicSource.volume = masterVolume * musicVolume;
                musicSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("BackgroundMusic not found in the library.");
        }
    }

    public enum AudioType
    {
        SFX,
        Music
    }
}