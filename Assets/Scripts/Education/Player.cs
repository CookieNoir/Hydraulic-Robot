using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;


public class LevelInfo
{
    public TaskCurrentValue[] LevelsValue;

    public LevelInfo(TaskCurrentValue[] levelsValue)
    {
        LevelsValue = levelsValue;
    }

    public override string ToString()
    {
        string text = null;
        foreach (var lvl in LevelsValue)
        {
            text += string.Format(lvl.currentValue + "/" +lvl.currentExtraValue + " ");
        }

        return text;
    }
}
public class Player : MonoBehaviour
{
    public LevelInfo levelInfo;
    public AllTasks AllTasks;
    public Player()
    {
        
    }
    public void SetLevelArray(TaskCurrentValue[] array)
    {
        levelInfo = new LevelInfo(array);
        load();
    }
    
    public void Save()
    {
        PlayerPrefs.SetString("SavedLevels", levelInfo.ToString());
    }

    public void load()
    {
        if (!PlayerPrefs.HasKey("SavedLevels"))
            return;
        /*
        string[] levels = PlayerPrefs.GetString("SavedLevels").Split();
        Debug.Log(PlayerPrefs.GetString("SavedLevels"));
        Debug.Log(levels);
        int length = levelConfig.Levels.Length;
        for(int i = 0; i < length;i++)
        {
            levelConfig.Levels[i].currentValue = Convert.ToInt32(levels[i].ToString());
        }*/

    }
    
}
