using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] GameObject mainMenuBackground;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] ConfigSystem configSystem;

    public AudioSource src;
    public AudioClip click;

    public GameObject panel;

    public Button newGameButton;
    public Button continueButton;

    public bool PreGameCutscene = false;
    public bool isInSettings = false;

    float musicVolume = 100f;
    float sfxVolume = 100f;

    public float MusicVolume
    {
        get { return musicVolume; }
        set
        {
            if (value >= 0 && value <= 100)
            {
                musicVolume = value;
                configSystem.SaveConfig();
            }
        }
    }
    public float SFXVolume
    {
        get { return sfxVolume; }
        set
        {
            if (value >= 0 && value <= 100)
            {
                sfxVolume = value;
                configSystem.SaveConfig();
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInSettings)
            {
                ReturnFromSettings();
            }
        }
    }

    private void Start()
    {
        configSystem.LoadConfig();
        panel.SetActive(true);
    }

    public void NewGame()
    {
        src.PlayOneShot(click);
        SceneManager.LoadScene("TestingScene");
    }

    public void GameContinue()
    {
        src.PlayOneShot(click);
    }

    public void Settings()
    {
        src.PlayOneShot(click);
        isInSettings = true;
        mainMenuBackground.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ReturnFromSettings()
    {
        mainMenuBackground.SetActive(true);
        isInSettings = false;
        src.PlayOneShot(click);
        settingsMenu.SetActive(false);
    }

    public void Quit()
    {
        src.PlayOneShot(click);
        StartCoroutine(_Quit());
    }

    private IEnumerator _Quit()
    {
        yield return new WaitForSeconds(click.length);
        Debug.Log("Application has quit");
        UnityEngine.Application.Quit();
    }

    public void NewGameSaveReset()
    {
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = value;
        Debug.Log("Music: " + MusicVolume);
        audioMixer.SetFloat("Music", Mathf.Log10(MusicVolume) * 20);
    }


    public void SetSFXVolume(float value)
    {
        SFXVolume = value;
        Debug.Log("SFX: " + SFXVolume);
        audioMixer.SetFloat("SFX", Mathf.Log10(SFXVolume) * 20);
    }
}
