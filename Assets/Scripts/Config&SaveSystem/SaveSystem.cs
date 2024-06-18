using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    string fullPath;
    private Difficulty currentDifficulty;
    private SceneManagement currentScene;
    bool fileIsUsed = false;

    public SceneManagement CurrentScene { get { return currentScene; } }

    public enum SceneManagement
    {
        Level_1,
        Level_1_Boss,
        Level_2,
        Leve_2_Boss,
        Level_3,
        Level_3_Boss,
        Level_4,
        Level_4_Boss
    }
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    public Difficulty CurrentDifficulty
    {
        get { return currentDifficulty; }
        set { currentDifficulty = value; }
    }

    public enum SaveID
    {
        First,
        Second,
        Third
    }

    private SaveID currentSaveID;

    public SaveID CurrentSaveID
    {
        get { return currentSaveID; }
        set
        {
            if (value == SaveID.First) { fullPath = Path.Combine(Application.persistentDataPath, ("save01")); currentSaveID = value; }
            else if (value == SaveID.Second) { fullPath = Path.Combine(Application.persistentDataPath, ("save02")); currentSaveID = value; }
            else if (value == SaveID.Third) { fullPath = Path.Combine(Application.persistentDataPath, ("save03")); currentSaveID = value;}
            else Debug.Log("CurrentSaveId is out of range");
        }
    }


    private void Start()
    {
        fullPath = Path.Combine(Application.persistentDataPath, ("save01"));
        currentScene = SceneManagement.Level_1_Boss; //HardCoded for testing
    }
    public void CreateSave(out bool completed) //man in the middle
    {
        if (!File.Exists(fullPath))
        {
            Save();
            completed = true;
        }
        else completed = false;
    }

    public void CreateSave()
    {
        if (!File.Exists(fullPath))
        {
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
            using (FileStream file = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write))
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
            using (FileStream file = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Read))
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

    public void DoesSaveFileExist(out bool saveEmpty01, out bool saveEmpty02, out bool saveEmpty03)
    {
        Debug.Log((Path.Combine(Application.persistentDataPath, "save01")));
        if (File.Exists(Path.Combine(Application.persistentDataPath, "save01"))) saveEmpty01 = false;
        else saveEmpty01 = true;
        if (File.Exists(Path.Combine(Application.persistentDataPath, "save02"))) saveEmpty02 = false;
        else saveEmpty02 = true;
        if (File.Exists(Path.Combine(Application.persistentDataPath, "save03"))) saveEmpty03 = false;
        else saveEmpty03 = true;
    }

    IEnumerator WaitingLine(bool saveFunction)
    {
        yield return new WaitForSeconds(1);
        if (saveFunction) Save();
        else Load();
    }
}
