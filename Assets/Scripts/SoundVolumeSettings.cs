using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundVolumeSettings : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    [SerializeField] private TextMeshProUGUI MasterVolumeText;
    [SerializeField] private TextMeshProUGUI SFXVolumeText;
    [SerializeField] private TextMeshProUGUI MusicVolumeText;

    private void Start()
    {
        // Check if AudioManager instance exists
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager instance is null!");
            return;
        }

        // Load saved volumes (default to 100 if no saved value exists)
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 100f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 100f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 100f);

        // Initialize AudioManager volumes
        OnMasterVolumeChanged(masterVolumeSlider.value);
        OnSFXVolumeChanged(sfxVolumeSlider.value);
        OnMusicVolumeChanged(musicVolumeSlider.value);

        // Add listeners to the sliders
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }

    private void OnMasterVolumeChanged(float value)
    {
        AudioManager.Instance.masterVolume = value / 100f;
        AudioManager.Instance.UpdateVolumes();
        MasterVolumeText.text = "Master Volume - " + value.ToString("0") + "%";
        PlayerPrefs.SetFloat("MasterVolume", value); // Save the value
    }

    private void OnSFXVolumeChanged(float value)
    {
        AudioManager.Instance.sfxVolume = value / 100f;
        AudioManager.Instance.UpdateVolumes();
        SFXVolumeText.text = "SFX Volume - " + value.ToString("0") + "%";
        PlayerPrefs.SetFloat("SFXVolume", value); // Save the value
    }

    private void OnMusicVolumeChanged(float value)
    {
        AudioManager.Instance.musicVolume = value / 100f;
        AudioManager.Instance.UpdateVolumes();
        MusicVolumeText.text = "Music Volume - " + value.ToString("0") + "%";
        PlayerPrefs.SetFloat("MusicVolume", value); // Save the value
    }
}