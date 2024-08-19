using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveSystem : MonoBehaviour
{
    //Variables for SaveSystem
    string fullPath;
    bool fileIsUsed = false;

    //private variables 
    private Difficulty currentDifficulty;
    private SceneManagement currentScene;
    private SaveID currentSaveID;
    private int timePlayed;

    //public variables
    public SceneManagement CurrentScene { get { return currentScene; } private set { currentScene = value; } }
    public Difficulty CurrentDifficulty
    {
        get { return currentDifficulty; }
        set { currentDifficulty = value; }
    }
    public SaveID CurrentSaveID
    {
        get { return currentSaveID; }
        set
        {
            if (value == SaveID.First) { fullPath = Path.Combine(Application.persistentDataPath, ("save01.dat")); currentSaveID = value; }
            else if (value == SaveID.Second) { fullPath = Path.Combine(Application.persistentDataPath, ("save02.dat")); currentSaveID = value; }
            else if (value == SaveID.Third) { fullPath = Path.Combine(Application.persistentDataPath, ("save03.dat")); currentSaveID = value; }
            else Debug.Log("!ERROR! CurrentSaveId  in SaveSystem is out of range");
        }
    }

    public int TimePlayed
    {
        get { return timePlayed; }
        set { if (value >= 0) timePlayed = value; }
    }
    //public Enums
    public enum SceneManagement
    {
        Camp,
        Level_1,
        Level_1_Boss,
        Level_2,
        Level_2_Boss,
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
    public enum SaveID
    {
        First,
        Second,
        Third
    }
    private void Start()
    {
        fullPath = Path.Combine(Application.persistentDataPath, ("save01.dat"));
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
                //Difficulty <value>
                switch (currentDifficulty)
                {
                    case Difficulty.Easy:
                        {
                            writer.Write(1);
                            break;
                        }
                    case Difficulty.Normal:
                        {
                            writer.Write(2);
                            break;
                        }
                    case Difficulty.Hard:
                        {
                            writer.Write(3);
                            break;
                        }
                    default: Debug.Log("!ERROR! Nepodarilo se zapsat 'currentDifficulty' do SaveFile " + currentSaveID); break;
                }
                //CurrentScene <Scene:*value*>
                writer.Write(TimePlayed);
                switch(currentScene)
                {
                    case SceneManagement.Camp:
                        {
                            writer.Write(00);
                            break;
                        }
                    case SceneManagement.Level_1:
                        {
                            writer.Write(01);
                            break;
                        }
                    case SceneManagement.Level_1_Boss:
                        {
                            writer.Write(02);
                            break;
                        }
                    case SceneManagement.Level_2:
                        {
                            writer.Write(03);
                            break;
                        }
                    case SceneManagement.Level_2_Boss:
                        {
                            writer.Write(04);
                            break;
                        }
                    case SceneManagement.Level_3:
                        {
                            writer.Write(05);
                            break;
                        }
                    case SceneManagement.Level_3_Boss:
                        {
                            writer.Write(06);
                            break;
                        }
                    case SceneManagement.Level_4:
                        {

                            writer.Write(07);
                            break;
                        }
                    case SceneManagement.Level_4_Boss:
                        {
                            writer.Write(08);
                            break;
                        }
                    default: Debug.Log("!ERROR! Could not save 'currentScene' from saveFile" + CurrentSaveID); break;
                }
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
                while (file.Position < file.Length)
                {
                    //line 00
                    int temp = reader.ReadInt32();
                    switch (temp)
                    {
                        case 1: CurrentDifficulty = Difficulty.Easy; break;
                        case 2: CurrentDifficulty = Difficulty.Normal; break;
                        case 3: currentDifficulty = Difficulty.Hard; break;
                        default: Debug.Log("!ERROR! could not load Difficulty"); break;
                    }
                    //line 04
                    timePlayed = reader.ReadInt32();
                    //line 08
                    int temp2 = reader.ReadInt32();
                    {
                        switch (temp2)
                        {
                            case 0: CurrentScene = SceneManagement.Camp; break;
                            case 1: CurrentScene = SceneManagement.Level_1; break;
                            case 2: CurrentScene = SceneManagement.Level_1_Boss; break;
                            case 3: CurrentScene = SceneManagement.Level_2; break;
                            case 4: CurrentScene = SceneManagement.Level_2_Boss; break;
                            case 5: CurrentScene = SceneManagement.Level_3; break;
                            case 6: CurrentScene = SceneManagement.Level_3_Boss; break;
                            case 7: CurrentScene = SceneManagement.Level_4; break;
                            case 8: CurrentScene = SceneManagement.Level_4_Boss; break;
                            default: Debug.Log("!ERROR! Could not load saved scene"); break;
                        }                            
                    }
                    //line 12
                }
            }
            fileIsUsed = false;
        }
        else StartCoroutine(WaitingLine(false));
    }

    public void DoesSaveFileExist(out bool saveEmpty01, out bool saveEmpty02, out bool saveEmpty03)
    {
        Debug.Log("Cesta k SaveFiles: " + (Path.Combine(Application.persistentDataPath, "save01")));
        if (File.Exists(Path.Combine(Application.persistentDataPath, "save01.dat"))) saveEmpty01 = false;
        else saveEmpty01 = true;
        if (File.Exists(Path.Combine(Application.persistentDataPath, "save02.dat"))) saveEmpty02 = false;
        else saveEmpty02 = true;
        if (File.Exists(Path.Combine(Application.persistentDataPath, "save03.dat"))) saveEmpty03 = false;
        else saveEmpty03 = true;
    }

    IEnumerator WaitingLine(bool saveFunction)
    {
        yield return new WaitForSeconds(1);
        if (saveFunction) Save();
        else Load();
    }
}
