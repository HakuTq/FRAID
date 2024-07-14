using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Globalization;

public class ConfigSystem : MonoBehaviour
{
    [SerializeField] MainMenu audioSettings;
    [SerializeField] Settings settings;
    [SerializeField] string fileName;
    string fullPath;
    bool fileIsUsed = false;
    //AllVariables
    float musicVolume = 0.8f, sfxVolume = 1f, refreshRate = 60;
    int screenWidth = 1080, screenHeight = 1920;
    bool vsync = true;
    bool fullscreen = true;
    public float MusicVolume
    {
        get { return musicVolume; }
        set 
        { 
            if (value >= 0 && value <= 100) musicVolume = value; 
            else Debug.Log("Couldnt set MusicVolume in ConfigSystem, value = " + value);
        }
    }
    public float SFXVolume
    {
        get { return sfxVolume; }
        set
        {
            if (value >= 0 && value <= 100) sfxVolume = value;
            else Debug.Log("Couldnt set SFXVolume in ConfigSystem, value = " + value);
        }
    }
    public int ScreenWidth
    {
        get { return screenWidth; }
        set { screenWidth = value; }
    }
    public int ScreenHeight
    {
        get { return screenHeight; }
        set { screenHeight = value; }
    }
    public float RefreshRate
    {
        get { return refreshRate; }
        set { refreshRate = value; }
    }
    public bool Vsync
    {
        get { return vsync; }
        set { vsync = value; }
    }
    public bool Fullscreen
    {
        get { return fullscreen; }
        set { fullscreen = value; }
    }
    private void Start()
    {
        fullPath = Path.Combine(Application.persistentDataPath, fileName); //persistentDataPath neboli Unity si to zaøídí samo :)
        Debug.Log("Config file path: " + fullPath);
        LoadConfig();
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
                    writer.Write("screen_width=");
                    writer.Write(ScreenWidth);
                    writer.WriteLine();
                    writer.Write("screen_height=");
                    writer.Write(ScreenHeight);
                    writer.WriteLine();
                    writer.Write("refresh_rate=");
                    writer.Write(RefreshRate);
                    writer.WriteLine();
                    writer.Write("vsync=");
                    if (vsync) writer.Write("true");
                    else writer.Write("false");
                    writer.Write("fulscreen=");
                    if (fullscreen) writer.Write("true");
                    else writer.Write("false");
                    writer.WriteLine();
                    writer.Write("music_volume=");
                    writer.Write(MusicVolume);
                    writer.WriteLine();
                    writer.Write("soundEffects_volume=");
                    writer.Write(SFXVolume);
                    writer.WriteLine();

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
                                            ScreenWidth = int.Parse(info[1]);
                                            break;
                                        }
                                    case "screen_height":
                                        {
                                            ScreenHeight = int.Parse(info[1]);
                                            break;
                                        }
                                    case "music_volume":
                                        {
                                            string fuckyoufloatparse = info[1].Replace(',','.');
                                            MusicVolume = float.Parse(fuckyoufloatparse, CultureInfo.InvariantCulture);
                                            break;
                                        }
                                    case "soundEffects_volume":
                                        {
                                            string fuckyoufloatparse = info[1].Replace(',', '.');
                                            SFXVolume = float.Parse(fuckyoufloatparse, CultureInfo.InvariantCulture);
                                            break;
                                        }
                                    case "fullscreen":
                                        {
                                            Fullscreen = bool.Parse(info[1]);
                                            break;
                                        }
                                    case "vsync":
                                        {
                                            Vsync = bool.Parse(info[1]);
                                            break;
                                        }
                                    case "refresh_rate":
                                        {
                                            string fuckyoufloatparse = info[1].Replace(',', '.');
                                            RefreshRate = float.Parse(fuckyoufloatparse, CultureInfo.InvariantCulture);
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
        LoadConfig();
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
