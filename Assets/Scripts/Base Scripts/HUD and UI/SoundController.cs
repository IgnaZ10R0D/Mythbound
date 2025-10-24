using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer; 
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private const string MasterPref = "MasterVolume";
    private const string MusicPref = "MusicVolume";
    private const string SFXPref = "SFXVolume";

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat(MasterPref, 0.75f);
        musicSlider.value = PlayerPrefs.GetFloat(MusicPref, 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat(SFXPref, 0.75f);

        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(MasterPref, masterSlider.value);
        PlayerPrefs.SetFloat(MusicPref, musicSlider.value);
        PlayerPrefs.SetFloat(SFXPref, sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume > 0 ? Mathf.Log10(volume) * 20 : -80f);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume > 0 ? Mathf.Log10(volume) * 20 : -80f);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume > 0 ? Mathf.Log10(volume) * 20 : -80f);
    }
}

