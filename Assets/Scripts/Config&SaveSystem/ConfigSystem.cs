using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor.Experimental.GraphView;

public class ConfigSystem : MonoBehaviour
{
    [SerializeField] MainMenu audioSettings;
    [SerializeField] string fileName;
    string fullPath;
    bool fileIsUsed = false;
    private void Start()
    {
        fullPath = Path.Combine(Application.persistentDataPath, fileName); //persistentDataPath neboli Unity si to zaøídí samo :)
    }
    public void SaveConfig()
    {
        fileIsUsed = true;
        if (!fileIsUsed)
        {
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    //writer.Write("screen_width=");
                    //writer.Write("screen_height=");
                    if (audioSettings != null)
                    {
                        writer.Write("music_volume=");
                        writer.Write(audioSettings.MusicVolume);
                        writer.WriteLine();
                        writer.Write("soundEffects_volume=");
                        writer.Write(audioSettings.SFXVolume);
                        writer.WriteLine();
                    }
                    else
                    {
                        writer.Write("music_volume=");
                        writer.Write("soundEffects_volume=");
                    }
                    //writer.Write("key_moveLeft=");
                    //writer.Write("key_moveRight=");
                    //writer.Write("key_jump=");
                    //writer.Write("key_moveDown=");
                    //writer.Write("key_attack=");
                    //writer.Write("key_dash=");
                }
            }
            fileIsUsed = false;
        }
        else StartCoroutine(waiter(0));
    }

    public void LoadConfig()
    {
        if (!fileIsUsed)
        {
            fileIsUsed = true;
            if (File.Exists(fullPath))
            {
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            char splitchar = '=';
                            string[] info = line.Split(splitchar); //info[0] = parametr; info[1] = data
                            if (info[1] != "")
                            {
                                switch (info[0])
                                {
                                    case "screen_width":
                                        {
                                            break;
                                        }
                                    case "screen_height":
                                        {
                                            break;
                                        }
                                    case "music_volume":
                                        {
                                            audioSettings.SetMusicVolume(float.Parse(info[1]));
                                            break;
                                        }
                                    case "soundEffects_volume":
                                        {
                                            audioSettings.SetSFXVolume(float.Parse(info[1]));
                                            break;
                                        }
                                    case "key_moveLeft":
                                        {
                                            break;
                                        }
                                    case "key_moveRight":
                                        {
                                            break;
                                        }
                                    case "key_jump":
                                        {
                                            break;
                                        }
                                    case "key_moveDown":
                                        {
                                            break;
                                        }
                                    case "key_attack":
                                        {
                                            break;
                                        }
                                    case "key_dash":
                                        {
                                            break;
                                        }
                                    default: break;
                                }
                            }
                        }
                    }
                }
                fileIsUsed = false;
            }
            else CreateConfig();
        }
        else StartCoroutine(waiter(1));
    }

    public void ResetConfig()
    {
        if (!fileIsUsed)
        {
            File.Delete(fullPath);
            CreateConfig();
        }
        else StartCoroutine(waiter(2));
    }
    private void CreateConfig() //Base Config
    {
        {
            using (FileStream stream = new FileStream(fullPath, FileMode.CreateNew))
            {
                using (StreamWriter writer = new StreamWriter(stream)) //posledni aby byl write
                {
                    writer.WriteLine("screen_width=1920");
                    writer.WriteLine("screen_height=1080");
                    writer.WriteLine("music_volume=80");
                    writer.WriteLine("soundEffects_volume=100");
                    writer.WriteLine("key_moveLeft=D");
                    writer.WriteLine("key_moveRight=A");
                    writer.WriteLine("key_jump=W");
                    writer.WriteLine("key_moveDown=S");
                    writer.WriteLine("key_attack=LeftMouseClick");
                    writer.Write("key_dash=SpaceBar");
                }
            }
        }
    }

    private IEnumerator waiter(int id) //Èasovaè aby se soubor mohl používat pouze jednou metodou
    {
        yield return new WaitForSeconds(1);
        switch(id)
        {
            case 0: SaveConfig(); break;
            case 1: LoadConfig(); break;
            case 2: ResetConfig(); break;
        }
    }
}
