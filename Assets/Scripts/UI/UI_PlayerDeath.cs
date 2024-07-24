using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_PlayerDeath : MonoBehaviour
{
    CanvasGroup mainCanvasGroup;
    [SerializeField] GameObject playerDeath;
    [SerializeField] float playerDeathCanvasEffect; //1 to 0
    void Start()
    {
        mainCanvasGroup = GetComponentInParent<CanvasGroup>();
        playerDeath.SetActive(false);
    }

    public void PlayerDeathUI()
    {
        mainCanvasGroup.alpha = playerDeathCanvasEffect;
        playerDeath.SetActive(true);
    }

    public void ResetButtonPlayerDeath()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    
}
