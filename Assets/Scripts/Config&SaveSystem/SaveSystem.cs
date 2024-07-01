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
            if (value == SaveID.First) { fullPath = Path.Combine(Application.persistentDataPath, ("save01")); currentSaveID = value; }
            else if (value == SaveID.Second) { fullPath = Path.Combine(Application.persistentDataPath, ("save02")); currentSaveID = value; }
            else if (value == SaveID.Third) { fullPath = Path.Combine(Application.persistentDataPath, ("save03")); currentSaveID = value; }
            else Debug.Log("CurrentSaveId is out of range");
        }
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
                //Difficulty <diff:*value*>
                switch (currentDifficulty)
                {
                    case Difficulty.Easy:
                        {
                            writer.Write("<diff:");
                            writer.Write(00);
                            writer.Write(">");
                            break;
                        }
                    case Difficulty.Normal:
                        {
                            writer.Write("<diff:");
                            writer.Write(01);
                            writer.Write(">");
                            break;
                        }
                    case Difficulty.Hard:
                        {
                            writer.Write("<diff:");
                            writer.Write(02);
                            writer.Write(">");
                            break;
                        }
                    default: Debug.Log("Nepodarilo se zapsat 'currentDifficulty' do SaveFile " + currentSaveID); break;
                }
                //CurrentScene <Scene:*value*>
                switch(currentScene)
                {
                    case SceneManagement.Camp:
                        {
                            writer.Write("<scene:");
                            writer.Write(00);
                            writer.Write(">");
                            break;
                        }
                    case SceneManagement.Level_1:
                        {
                            writer.Write("<scene:");
                            writer.Write(01);
                            writer.Write(">");
                            break;
                        }
                    case SceneManagement.Level_1_Boss:
                        {
                            writer.Write("<scene:");
                            writer.Write(02);
                            writer.Write(">");
                            break;
                        }
                    case SceneManagement.Level_2:
                        {
                            writer.Write("<Scene:");
                            writer.Write(03);
                            writer.Write(">");
                            break;
                        }
                    case SceneManagement.Level_2_Boss:
                        {
                            writer.Write("<scene:");
                            writer.Write(04);
                            writer.Write(">");
                            break;
                        }
                    case SceneManagement.Level_3:
                        {
                            writer.Write("<scene:");
                            writer.Write(05);
                            writer.Write(">");
                            break;
                        }
                    case SceneManagement.Level_3_Boss:
                        {
                            writer.Write("<Scene:");
                            writer.Write(06);
                            writer.Write(">");
                            break;
                        }
                    case SceneManagement.Level_4:
                        {
                            writer.Write("<scene:");
                            writer.Write(07);
                            writer.Write(">");
                            break;
                        }
                    case SceneManagement.Level_4_Boss:
                        {
                            writer.Write("<scene:");
                            writer.Write(08);
                            writer.Write(">");
                            break;
                        }
                    default: Debug.Log("Could not save 'currentScene' from saveFile" + CurrentSaveID); break;
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
                    bool end = false;
                    string variable;
                    do
                    {
                        variable = reader.ReadChar().ToString();
                        if (variable[0] == '<')
                        {
                            while (!variable.EndsWith(':')) variable += reader.ReadChar();
                            switch (variable)
                            {
                                case "diff:":
                                    {
                                        int value = reader.ReadInt32();
                                        currentDifficulty = (Difficulty)value;
                                        break;
                                    }
                                case "scene:":
                                    {
                                        int value = reader.ReadInt32();
                                        CurrentScene = (SceneManagement)value;
                                        break;
                                    }
                                default: Debug.Log("Could not load variable"); break;
                            }
                        }
                        else if (variable.EndsWith('>')) end = true;
                    } while (!end);
                }
                /*int temp = reader.ReadInt32();
                switch(temp)
                {
                    case 1: CurrentDifficulty = Difficulty.Easy; break;
                    case 2: CurrentDifficulty = Difficulty.Normal; break;
                    case 3: currentDifficulty = Difficulty.Hard; break;
                    default: Debug.Log("Nelze nacist Current Difficulty v SaveSystemu/Load"); break;
                }*/
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
