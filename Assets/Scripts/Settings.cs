using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicVolSlider;
    [SerializeField] private Slider SFXVolSlider;
    ConfigSystem config;

    public TMPro.TMP_Dropdown resolutionDropdown;

    public AudioSource src;
    public AudioClip srcOne;

    Resolution[] resolutions;

    void Start()
    {
        config = FindFirstObjectByType<ConfigSystem>();
        if (config != null)
        {
            SetMusicVolume(config.MusicVolume);
            SetSFXVolume(config.SFXVolume);
        }
        else
        {
            Debug.Log("Settings.cs nenasel ConfigSystem");
            SetMusicVolume();
            SetSFXVolume();
        }
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        config.ScreenHeight = resolution.height;
        config.ScreenWidth = resolution.width;
        config.SaveConfig();
        if (config.Fullscreen) Screen.SetResolution(config.ScreenWidth, config.ScreenHeight, true);
        else Screen.SetResolution(config.ScreenWidth, config.ScreenHeight, false);
    }

    public void SetMusicVolume()
    {
        audioMixer.SetFloat("Music", Mathf.Log10(musicVolSlider.value) * 20);
    }

    public void SetMusicVolume(float value)
    {
        musicVolSlider.value = value;
        audioMixer.SetFloat("Music", Mathf.Log10(value) * 20);
    }
    public void SetSFXVolume()
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(SFXVolSlider.value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        SFXVolSlider.value = value;
        audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}