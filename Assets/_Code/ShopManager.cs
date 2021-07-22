using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    public Text tokensCount;
    public Text unlimitedLives, withoutAdsWeek;
    public float secondsUnlimitedLives = 86400f;
    //public float seconsWithoutAdsWeek = 604800f;
    public Button BuyUnlimitedLivesButton, BuyWithoutAdsButton;
    public Text WithoutAdsStatusBuy;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
    public void StartGame()
    {
        EventManager.instance.CheckCountTokensAction += UpdateTokens;
        UpdateTokens(SavedData.instance.tokensCount,0);
        CheckStatusUnlimitedLives();
        CheckStatusWithoutAds();
        //CheckStatusWithoutAdsWeek();
    }
    public void CheckStatusWithoutAds()
    {
        if(SavedData.instance.removeAd)
        {
            WithoutAdsStatusBuy.text = "Subscription issued!";
            BuyWithoutAdsButton.interactable = false;
        }
        else
        {
            WithoutAdsStatusBuy.text = "";
        }
    }
    private void UpdateTokens(int _newCountTokens, int _newCount)
    {
        tokensCount.text = _newCountTokens.ToString();
    }
    private void CheckStatusUnlimitedLives()
    {
        ulong diff = ((ulong)DateTime.Now.Ticks - SavedData.instance.lastBuyUnlimitedLives);
        ulong m = diff / TimeSpan.TicksPerSecond;
        float secondLeft = (float)(secondsUnlimitedLives - m);
        if (secondLeft > 0)
        {
            string _t = "";
            int _hour = ((int)secondLeft / 3600);
            _t += _hour.ToString("00") + ":";
            if (_hour >= 1)
            {
                unlimitedLives.color = Color.green;
            }
            else
            {
                unlimitedLives.color = Color.red;
            }
            _t += (((int)secondLeft / 60) % 60).ToString("00") + ":";
            _t += ((int)secondLeft % 60).ToString("00");
            BuyUnlimitedLivesButton.interactable = false;
            unlimitedLives.text = _t;
        }
        else
        {
            unlimitedLives.color = Color.red;
            BuyUnlimitedLivesButton.interactable = true;
            unlimitedLives.text = "00:00:00";
            SavedData.instance.unlimitedLives = false;
            SavedData.instance.Saved();
        }
    }
    private void Update()
    {
        if(!ScreenManager.instance.screens[0].activeSelf) { return; }

        CheckStatusUnlimitedLives();
        //CheckStatusWithoutAdsWeek();
    }
}
