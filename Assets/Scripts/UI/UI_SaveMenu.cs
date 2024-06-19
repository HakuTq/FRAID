using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class UI_SaveMenu : MonoBehaviour
{
    //Did this shit while drunk in Random Ass City in Sweden :)
    [SerializeField] SaveSystem saveSystem;
    [SerializeField] Button[] saveFileSelector;
    [SerializeField] TextMeshProUGUI[] buttonText;
    [SerializeField] GameObject saveMenu;
    [SerializeField] GameObject[] newGameButton;
    [SerializeField] GameObject confirm;
    [SerializeField] Button confirmYes;
    [SerializeField] Button confirmCancel;

    bool[] saveEmpty;
    bool optionConfirmed = false;
    bool optionCanceled = false;

    public void Button01Clicked()
    {
        saveSystem.CurrentSaveID = SaveSystem.SaveID.First;
        if (saveEmpty[0])
        {
            bool completed;
            saveSystem.CreateSave(out completed);
            if (completed)
            {
                //SceneManager.LoadScene("NewGame");
                saveMenu.SetActive(false);
            }
            else Debug.Log("Nepodarilo se vytvorit SaveFile01");
        }
        else
        {
            SceneManager.LoadScene(saveSystem.CurrentScene.ToString());
            Debug.Log("SaveFile01 se nacetl uspesne");
        }
    }

    public void Button02Clicked()
    {
        saveSystem.CurrentSaveID = SaveSystem.SaveID.Second;
        if (saveEmpty[1])
        {
            bool completed;
            saveSystem.CreateSave(out completed);
            if (completed)
            {
                //SceneManager.LoadScene("NewGame");
                saveMenu.SetActive(false);
            }
            else Debug.Log("Nepodarilo se vytvorit SaveFile02");
        }
        else
        {
            SceneManager.LoadScene(saveSystem.CurrentScene.ToString());
            Debug.Log("SaveFile02 se nacetl uspesne");
        }
    }

    public void Button03Clicked()
    {
        saveSystem.CurrentSaveID = SaveSystem.SaveID.Third;
        if (saveEmpty[2])
        {
            bool completed;
            saveSystem.CreateSave(out completed);
            if (completed)
            {
                //SceneManager.LoadScene("NewGame");
                saveMenu.SetActive(false);
            }
            else Debug.Log("Nepodarilo se vytvorit SaveFile03");
        }
        else
        {
            SceneManager.LoadScene(saveSystem.CurrentScene.ToString());
            Debug.Log("SaveFile03 se nacetl uspesne");
        }
    }

    public void Button01NewGame(bool optionAlreadySelected)
    {
        confirm.SetActive(true);
        if (!optionAlreadySelected) StartCoroutine(ConfirmWaiting(1));
        else if (optionConfirmed)
        {
            saveSystem.CurrentSaveID = SaveSystem.SaveID.First;
            saveSystem.ResetSave();
            SceneManager.LoadScene(saveSystem.CurrentScene.ToString());
        }
        else if (optionCanceled) confirm.SetActive(false);
    }

    public void Button02NewGame(bool optionAlreadySelected)
    {
        confirm.SetActive(true);
        if (!optionAlreadySelected) StartCoroutine(ConfirmWaiting(2));
        else if (optionConfirmed)
        {
            saveSystem.CurrentSaveID = SaveSystem.SaveID.Second;
            saveSystem.ResetSave();
            SceneManager.LoadScene(saveSystem.CurrentScene.ToString());
        }
        else if (optionCanceled) confirm.SetActive(false);
    }

    public void Button03NewGame(bool optionAlreadySelected)
    {
        confirm.SetActive(true);
        if (!optionAlreadySelected) StartCoroutine(ConfirmWaiting(3));
        else if (optionConfirmed)
        {
            saveSystem.CurrentSaveID = SaveSystem.SaveID.Third;
            saveSystem.ResetSave();
            SceneManager.LoadScene(saveSystem.CurrentScene.ToString());
        }
        else if (optionCanceled) confirm.SetActive(false);
    }


    IEnumerator ConfirmWaiting(int id)
    {
        while (!optionConfirmed && !optionCanceled) yield return null;
        switch (id)
        {
            case 1: Button01NewGame(true); break;
            case 2: Button02NewGame(true); break;
            case 3: Button03NewGame(true); break;
            default: break;
        }
        optionCanceled = false;
        optionConfirmed = false;
    }

    public void OptionConfirm() { optionConfirmed = true; }
    public void OptionCancel() { optionCanceled = true; }

    public void ShowSaveMenu()
    {
        saveMenu.SetActive(true);
        CheckSavedGames();
    }

    public void CheckSavedGames()
    {
        foreach (GameObject a in newGameButton) a.SetActive(false);
        saveEmpty = new bool[saveFileSelector.Count()];
        saveSystem.DoesSaveFileExist(out saveEmpty[0], out saveEmpty[1], out saveEmpty[2]);
        //3 ifs jednoho dne dat do cyklu
        if (saveEmpty[0])
        {
            buttonText[0].text = "New Game";
        }
        else
        {
            saveSystem.CurrentSaveID = SaveSystem.SaveID.First;
            buttonText[0].text = "Difficutly: " + saveSystem.CurrentDifficulty.ToString();
            newGameButton[0].SetActive(true);
        }
        if (saveEmpty[1])
        {
            buttonText[1].text = "New Game";
        }
        else
        {
            saveSystem.CurrentSaveID = SaveSystem.SaveID.Second;
            buttonText[1].text = "Difficutly: " + saveSystem.CurrentDifficulty.ToString();
            newGameButton[1].SetActive(true);
        }
        if (saveEmpty[2])
        {
            buttonText[2].text = "New Game";
        }
        else
        {
            saveSystem.CurrentSaveID = SaveSystem.SaveID.Third;
            buttonText[2].text = "Difficutly: " + saveSystem.CurrentDifficulty.ToString();
            newGameButton[2].SetActive(true);
        }
    }

    public void CancelSaveMenu() { saveMenu.SetActive(false); }
}
