using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    public static AdManager instance;
    public GarageManager garageManager;
    private static string playStoreId = "4219995";
    private static string appleStoreId = "4219994";
    private static string interstitialAdAndroid = "Interstitial_Android";
    private static string rewardedVideoAdAndroid = "Rewarded_Android";
    public bool testMode;
    public bool playStore;
    public enum TypeRewardAds
    {
        moreTimeCount, moreFuelForCurrentCar, moreLives, restartLevelAndPlusLive
    }
    public TypeRewardAds typeRewardAds;
    public int countAd = 0;
    public int[] buyContinueGame;
    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
    }
    private void Start()
    {
        InitializeAdvertisement();
        LoadRewardAd();
    }
    private  void InitializeAdvertisement()
    {
        if(playStore)
        {
            Advertisement.Initialize(playStoreId, testMode);
            Advertisement.AddListener(this);
            return;
        }
        Advertisement.Initialize(appleStoreId, testMode);
        Advertisement.AddListener(this);
    }
    public void ShowAddMoreFuelForCurrentCar()
    {
        if(DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].fuelCount < DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].maxCountFuel)
        {
            typeRewardAds = TypeRewardAds.moreFuelForCurrentCar;
            Advertisement.Show(rewardedVideoAdAndroid);
        }
    }
    public void ShowAdMoreLives()
    {
        if (SavedData.instance.livesCount < SavedData.instance.maxLivesCount)
        {
            Advertisement.Show(rewardedVideoAdAndroid);
            typeRewardAds = TypeRewardAds.moreLives;
        }
    }
    public void ShowInterAd()
    {
         if(!Advertisement.IsReady(interstitialAdAndroid) && !SavedData.instance.removeAd)
        {
            return;
        }
        Advertisement.Show(interstitialAdAndroid);
    }
    public void ShowAdRestartLevel()
    {
        if(!Advertisement.IsReady(rewardedVideoAdAndroid))
        {
            return;
        }
        if (countAd < 3)
        {
            GameManager.instance.gameOverPanel.SetTrigger("Close");
            typeRewardAds = TypeRewardAds.restartLevelAndPlusLive;
            countAd++;
        }
    }
    public void BuyContinueGame()
    {
        if(countAd < 3)
        {
            if (SavedData.instance.tokensCount >= buyContinueGame[countAd])
            {
                //Debug.Log("buyContinue: " + buyContinueGame[countAd]);
                SavedData.instance.tokensCount -= buyContinueGame[countAd];
                countAd++;
                SavedData.instance.Saved();
                LevelManager.instance.timeCount = 7;
                LevelManager.instance.statusCurrentLevel = StatusCurrentLevel.process;
                GameManager.instance.gameOverPanel.SetTrigger("Close");
                GameManager.instance.ContinueGame();
            }
        }
    }
    private void LoadRewardAd()
    {
        Advertisement.Load(rewardedVideoAdAndroid);
    }
    public void OnUnityAdsReady(string placementId)
    {

    }
    public void OnUnityAdsDidError(string placementId)
    {

    }
    public void OnUnityAdsDidStart(string placementId)
    {

    }
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch(showResult)
        {
            case ShowResult.Failed:
            break;
            case ShowResult.Skipped:
            break;
            case ShowResult.Finished:
            if(placementId == rewardedVideoAdAndroid)
            {
                if (typeRewardAds == TypeRewardAds.moreTimeCount)
                {
                    LevelManager.instance.timeCount = 13;
                    LevelManager.instance.statusCurrentLevel = StatusCurrentLevel.process;
                    countAd++;
                    GameManager.instance.gameOverPanel.SetTrigger("Close");
                    GameManager.instance.ContinueGame();
                }
                else if(typeRewardAds == TypeRewardAds.moreFuelForCurrentCar)
                {
                    float _coefficient_0 = DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].maxCountFuel / 100f;
                    float _coefficient_1 = _coefficient_0 * 33f;
                    DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].fuelCount += _coefficient_1;
                    if (DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].fuelCount > DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].maxCountFuel)
                    {
                        DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].fuelCount = DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].maxCountFuel;
                    }
                    garageManager.AllStatusForCurrentCar();
                    garageManager.startLevelButton.interactable = true;
                    SavedData.instance.Saved();
                    DataManager.instance.Saved();
                    garageManager.CheckCurrectCarFuelCount();
                }
                else if(typeRewardAds == TypeRewardAds.moreLives)
                {
                    EventManager.instance.CheckCountLives('+', 1);
                    SavedData.instance.Saved();
                }
                else if(typeRewardAds == TypeRewardAds.restartLevelAndPlusLive)
                {
                    EventManager.instance.CheckCountLives('+', 1);
                    ScreenManager.instance.screenLoad.SetTrigger("Load");
                    LevelManager.instance.StartLevel();
                    //restart level
                }
            }
            LoadRewardAd();
            break;
        }
    }
}
