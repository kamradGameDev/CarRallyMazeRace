using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LevelStatusManager : MonoBehaviour
{
    public static LevelStatusManager instance;
    public bool[] statusLevelComply;
    private void Start()
    {
        if(!instance)
        {
            instance = this;
        }
        if (File.Exists(Application.persistentDataPath + "Progress.json"))
        {
            Load();
        }
        else
        {
            GameManager.instance.selectedLevelManager.StatusButtons(0);
        }
        
    }

    private void Load()
    {
        string json = ReadFromFile();
        LoadData loadData = JsonUtility.FromJson<LoadData>(json);
        for(int i = 0; i < statusLevelComply.Length; i++)
        {
            statusLevelComply[i] = loadData.statusLevelComply[i];
            if(statusLevelComply[i])
            {
                GameManager.instance.selectedLevelManager.StatusButtons(i);
            }
        }
    }
    public void Saved()
    {
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "Progress.json");
        writer.WriteLine(JsonUtility.ToJson(this));
        writer.Close();
    }

    private string ReadFromFile()
    {
        using (StreamReader reader = new StreamReader(Application.persistentDataPath + "Progress.json"))
        {
            string json = reader.ReadToEnd();
            return json;
        }
    }
    private class LoadData
    {
        public bool[] statusLevelComply;
    }
}
