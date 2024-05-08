using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class MainMenu : MonoBehaviour
{
    Settings settings;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] GameObject mainMenuBackground;
    [SerializeField] GameObject settingsMenu;

    public AudioSource src;
    public AudioClip click;

    public Animator transition;

    public GameObject panel;

    public Button newGameButton;
    public Button continueButton;

    public bool PreGameCutscene = false;
    public bool isInSettings = false;

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
        SettingsLoad();
        //tady bude animace transition (fade in)
        panel.SetActive(true);
    }

    private void SettingsLoad()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            settings.SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("SFXvolume"))
        {
            settings.SetSFXVolume();
        }
    }

    public void NewGame()
    {
        src.PlayOneShot(click);
        NewGameSaveReset();
        PlayerPrefs.SetInt("HasASavedGame", 1);
        PlayerPrefs.Save();
        //transition animace fade out
        SceneManager.LoadScene("Level1"); //zatim level1, uvidime jak se to bude jmenovat pozdìjc
    }

    public void GameContinue()
    {
        src.PlayOneShot(click);
        //animace fade out
        SceneManager.LoadScene(PlayerPrefs.GetString("Level"));
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

    private IEnumerator Quit()
    {
        src.PlayOneShot(click);
        yield return new WaitForSeconds(click.length);
        Debug.Log("Application has quit");
        UnityEngine.Application.Quit();
    }

    public void NewGameSaveReset()
    {
        PlayerPrefs.DeleteKey("Level");
        //tady toho bude urèitì víc, èasem
    }
}
