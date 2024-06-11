using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    string fullPath = Path.Combine(Application.persistentDataPath, ("save01"));
    string currentSaveID = "01";
    private Difficulty currentDifficulty;
    bool fileIsUsed = false;
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    public Difficulty CurrentDifficulty
    {
        get { return currentDifficulty; }
    }

    public string CurrentSaveID
    {
        get { return currentSaveID; }
        set 
        { 
            if (value == "01" || value == "02" || value == "03")
            {
                currentSaveID = value;
                fullPath = Path.Combine(Application.persistentDataPath, ("save" + currentSaveID));
            }
        }
    }

    public void CreateSave(out bool completed)
    {
        if (!File.Exists(fullPath))
        {
            File.Create(fullPath);
            Save();
            completed = true;
        }
        else completed = false;
    }

    void CreateSave()
    {
        if (!File.Exists(fullPath))
        {
            File.Create(fullPath);
            Save();
        }
    }

    public void ResetSave()
    {
        File.Delete(fullPath);
        CreateSave();
    }

    public void Save()
    {
        if (!fileIsUsed)
        {
            fileIsUsed = true;
            using (FileStream file = new FileStream(fullPath, FileMode.Open, FileAccess.Write))
            {

            }
            fileIsUsed = false;
        }
        else StartCoroutine(waitingLine(true));
    }

    public void Load()
    {
        if (!fileIsUsed)
        {
            fileIsUsed = true;
            using (FileStream file = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {

            }
            fileIsUsed = false;
        }
        else StartCoroutine(waitingLine(false));
    }

    public void DoesSaveFileExist(out bool save01, out bool save02, out bool save03)
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, "save01"))) save01 = true;
        else save01 = false;
        if (File.Exists(Path.Combine(Application.persistentDataPath, "save02"))) save02 = true;
        else save02 = false;
        if (File.Exists(Path.Combine(Application.persistentDataPath, "save03"))) save03 = true;
        else save03 = false;
    }

    IEnumerator waitingLine(bool save)
    {
        yield return new WaitForSeconds(1);
        if (save) Save();
        else Load();
    }
}
