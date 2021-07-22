using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    [System.Serializable]
    public struct Cars
    {
        public bool statusCar;
        public bool[] Colors;
        public bool[] horns;
        public int currentHorn;
        public bool Spoiler;
        public bool darkenedWindow;
        public int currentColor;
        public float fuelCount;
        public float maxCountFuel;
    }
    public Cars[] cars = new Cars[20];
    public bool[] LicensePlate;
    public int currentLicensePlate;
    public int currentCar;
    public int currentHorn;

    private void Start()
    {
        if(!instance)
        {
            instance = this;
        }
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].Colors = new bool[7];
        }
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].horns = new bool[6];
        }
        if (File.Exists(Application.persistentDataPath + "Data.json"))
        {
            Load();
        }
        else
        {
            LicensePlate = new bool[CustomizationManager.instance.licensePlate.Length];
            LicensePlate[0] = true;
            for (int i = 0; i < cars.Length; i++)
            {
                cars[i].Colors = new bool[7];
            }
            for(int i = 0; i < cars.Length; i++)
            {
                for(int j = 0; j < cars[i].Colors.Length; j++)
                {
                    if(j == 0)
                    {
                        cars[i].Colors[j] = true;
                    }
                }
            }
            Saved();
        }
    }

    private void Load()
    {
        string json = ReadFromFile();
        LoadData loadData = JsonUtility.FromJson<LoadData>(json);
        for(int i = 0; i < cars.Length; i++)
        {
            for(int j = 0; j < cars[i].Colors.Length; j++)
            {
                cars[i].Colors[j] = loadData.cars[i].Colors[j];

            }
            cars[i].statusCar = loadData.cars[i].statusCar;
            cars[i].Spoiler = loadData.cars[i].Spoiler;
            cars[i].horns = loadData.cars[i].horns;
            cars[i].currentHorn = loadData.cars[i].currentHorn;
            cars[i].darkenedWindow = loadData.cars[i].darkenedWindow;
            cars[i].currentColor = loadData.cars[i].currentColor;
            cars[i].fuelCount = loadData.cars[i].fuelCount;
            cars[i].maxCountFuel = loadData.cars[i].maxCountFuel;
        }
        LicensePlate = loadData.LicensePlate;
        currentLicensePlate = loadData.currentLicensePlate;
        currentCar = loadData.currentCar;
        currentHorn = loadData.currentHorn;
        CustomizationManager.instance.licensePlate[currentLicensePlate].SetActive(true);
        CustomizationManager.instance.currentIndexLicensePlate = currentLicensePlate;
        CustomizationManager.instance.currentIndexHorn = currentHorn;
        for (int i = 0; i < cars.Length; i++)
        {
            for(int j = 0; j < CustomizationManager.instance.cars[i].transform.childCount; j++)
            {
                CustomizationManager.instance.cars[i].transform.GetChild(j).gameObject.SetActive(false);
                CustomizationManager.instance.cars[i].transform.GetChild(cars[i].currentColor).gameObject.SetActive(true);
                if (cars[i].Spoiler)
                {
                    CustomizationManager.instance.cars[i].transform.GetChild(j).GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    CustomizationManager.instance.cars[i].transform.GetChild(j).GetChild(1).gameObject.SetActive(false);
                }
                if(cars[i].darkenedWindow)
                {
                    CustomizationManager.instance.cars[i].transform.GetChild(j).GetChild(0).GetChild(0).gameObject.SetActive(false);
                    CustomizationManager.instance.cars[i].transform.GetChild(j).GetChild(0).GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    CustomizationManager.instance.cars[i].transform.GetChild(j).GetChild(0).GetChild(0).gameObject.SetActive(true);
                    CustomizationManager.instance.cars[i].transform.GetChild(j).GetChild(0).GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }
    public void Saved()
    {
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "Data.json");
        writer.WriteLine(JsonUtility.ToJson(this));
        writer.Close();
    }

    private string ReadFromFile()
    {
        using(StreamReader reader = new StreamReader(Application.persistentDataPath + "Data.json"))
        {
            string json = reader.ReadToEnd();
            return json;
        }
    }
    private class LoadData
    {
        [System.Serializable]
        public struct Cars
        {
            public bool statusCar;
            public bool[] Colors;
            public bool[] horns;
            public int currentHorn;
            public bool Spoiler;
            public bool darkenedWindow;
            public int currentColor;
            public float fuelCount;
            public float maxCountFuel;
        }
        public Cars[] cars = new Cars[20];
        public bool[] LicensePlate;
        public int currentLicensePlate;
        public int currentCar;
        public int currentHorn;
    }
}
