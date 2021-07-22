using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SavedData : MonoBehaviour
{
    public static SavedData instance;
    public int livesCount, maxLivesCount;
    public int tokensCount;
    public bool removeAd = false;
    public ulong lastBuyUnlimitedLives, lastBuyWithoutAdsPerWeek;
    public bool unlimitedLives;
    private void Start()
    {
        if(!instance)
        {
            instance = this;
        }
        if(File.Exists(Application.persistentDataPath + "DataTokens.json"))
        {
            Load();
        }
        else
        {
            Saved();
        }
        ShopManager.instance.StartGame();
        GameManager.instance.StartGame();
        GameManager.instance.selectedLevelManager.StartGame();
        EventManager.instance.CheckCountLives(' ', livesCount);
    }
    private void Load()
    {
        string json = ReadFromFile();
        LoadData loadData = JsonUtility.FromJson<LoadData>(json);
        tokensCount = loadData.tokensCount;
        removeAd = loadData.removeAd;
        lastBuyUnlimitedLives = loadData.lastBuyUnlimitedLives;
        unlimitedLives = loadData.unlimitedLives;
        livesCount = loadData.livesCount;
        maxLivesCount = loadData.maxLivesCount;
        lastBuyWithoutAdsPerWeek = loadData.lastBuyWithoutAdsPerWeek;
        EventManager.instance.CheckCountTokens(tokensCount, CustomizationManager.instance.countBuyItems);
    }
    public void Saved()
    {
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "DataTokens.json");
        writer.WriteLine(JsonUtility.ToJson(this));
        writer.Close();
    }
    private string ReadFromFile()
    {
        using (StreamReader reader = new StreamReader(Application.persistentDataPath + "DataTokens.json"))
        {
            string json = reader.ReadToEnd();
            return json;
        }
    }
    private class LoadData
    {
        public int tokensCount;
        public int livesCount, maxLivesCount;
        public bool removeAd;
        public ulong lastBuyUnlimitedLives, lastBuyWithoutAdsPerWeek;
        public bool unlimitedLives;
    }
}
