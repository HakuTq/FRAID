using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    string fullPath;
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
        set {  currentDifficulty = value; }
    }

    public enum SaveID
    {
        First,
        Second,
        Third
    }

    private SaveID currentSaveID = SaveID.First;

    public SaveID CurrentSaveID
    {
        get { return currentSaveID; }
        set { currentSaveID = value; }
    }


    private void Start()
    {
        fullPath = Path.Combine(Application.persistentDataPath, ("save01"));
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
                BinaryWriter writer = new BinaryWriter(file);
                if (CurrentDifficulty == Difficulty.Easy) writer.Write(01);
                else if (CurrentDifficulty == Difficulty.Normal) writer.Write(02);
                else if (CurrentDifficulty == Difficulty.Hard) writer.Write(03);
                else Debug.Log("Nepodarilo se zapsat Current Difficulty do Save Filu SaveSystem/Save");
            }
            fileIsUsed = false;
        }
        else StartCoroutine(WaitingLine(true));
    }

    public void Load()
    {
        if (!fileIsUsed)
        {
            fileIsUsed = true;
            using (FileStream file = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader reader = new BinaryReader(file);
                int temp = reader.ReadInt32();
                switch(temp)
                {
                    case 1: CurrentDifficulty = Difficulty.Easy; break;
                    case 2: CurrentDifficulty = Difficulty.Normal; break;
                    case 3: currentDifficulty = Difficulty.Hard; break;
                    default: Debug.Log("Nelze nacist Current Difficulty v SaveSystemu/Load"); break;
                }
            }
            fileIsUsed = false;
        }
        else StartCoroutine(WaitingLine(false));
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

    IEnumerator WaitingLine(bool saveFunction)
    {
        yield return new WaitForSeconds(1);
        if (saveFunction) Save();
        else Load();
    }
}
