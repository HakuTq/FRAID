using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;

    public AudioSource src;
    public AudioClip srcOne;

    public static bool GameIsPaused = false;
    public static bool IsInSettings = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                if (!IsInSettings)
                {
                    Resume();
                }
                else
                {
                    ReturnFromSettings();
                }
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        src.PlayOneShot(srcOne);
        _Pause();
    }

    private void _Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        GameIsPaused = true;
    }

    public void Resume()
    {
        src.PlayOneShot(srcOne);
        _Resume();
    }

    private void _Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        GameIsPaused = false;
    }

    public void Settings()
    {
        src.PlayOneShot(srcOne);
        _Settings();
    }

    public void _Settings()
    {
        IsInSettings = true;
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ReturnFromSettings()
    {
        src.PlayOneShot(srcOne);
        _ReturnFromSettings();
    }

    private void _ReturnFromSettings()
    {
        IsInSettings = false;
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Quit()
    {
        src.PlayOneShot(srcOne);
        _Quit();
    }

    private void _Quit()
    {
        Debug.Log("Application has quit");
        Application.Quit();
    }

    public void MMenu()
    {
        src.PlayOneShot(srcOne);
        _MMenu();
    }


    private void _MMenu()
    {
        Debug.Log("Returning to main menu");
        //yield return new WaitForSeconds(0.01f);
        SceneManager.LoadScene("MainMenu");
    }
}