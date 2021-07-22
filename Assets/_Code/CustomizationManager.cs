using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationManager : MonoBehaviour
{
    public static CustomizationManager instance;
    public Text BuyPanelDescriptionText;
    public GameObject[] licensePlate;
    public int countBuyItems;
    public GarageManager garageManager;
    public GameObject[] cars = new GameObject[20];
    public CurrentCarIndexInMenu currentCarIndexInMenu;
    public Animator customButton, shopButton, buyCar, backToMenu, Customize_window, LeftAndRightButtons;
    public int countCustomizeColor;
    public int countLicensePlate;
    public int currentIndexColor;
    public int currentIndexLicensePlate;
    public int currentIndexHorn;
    public int currentCarIndex;
    public bool spoilerSelected = false;
    public bool darkenedWindowSelected = false;
    public int countPriceColor;
    public int countPriceLicensePlate;
    public int countPriceHorn;
    public CarGarageHelper[] carGarageHelpers;
    public bool scrollStatus = true;
    public bool backInShop;
    //last state items car
    public int lastColor, lastStatusLicencePlate, lastHorn;
    public bool lastStatusSpoiler, lastStatusDarkenedWindow;
    
    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
        countLicensePlate = licensePlate.Length;
    }
   
    public void BuyItemsForCurrentCar()
    {
        if(SavedData.instance.tokensCount >= countBuyItems)
        {
            carGarageHelpers[currentCarIndex].startCustomCurrentCar = false;
            Customize_window.SetTrigger("Close");
            customButton.SetTrigger("Open");
            shopButton.SetTrigger("Open");
            backToMenu.SetTrigger("Open");
            garageManager.playThisCar.SetTrigger("Open");
            LeftAndRightButtons.SetTrigger("Open");
            SavedData.instance.tokensCount -= countBuyItems;
            DataManager.instance.cars[currentCarIndex].currentColor = currentIndexColor;
            DataManager.instance.cars[currentCarIndex].Spoiler = spoilerSelected;
            DataManager.instance.cars[currentCarIndex].darkenedWindow = darkenedWindowSelected;
            DataManager.instance.currentLicensePlate = currentIndexLicensePlate;
            DataManager.instance.LicensePlate[currentIndexLicensePlate] = true;
            DataManager.instance.cars[currentCarIndex].Colors[currentIndexColor] = true;
            DataManager.instance.cars[currentCarIndex].horns[currentIndexHorn] = true;
            SavedData.instance.Saved();
            DataManager.instance.Saved();
            garageManager.buyPanel.SetTrigger("Close");
            garageManager.GetComponent<Image>().raycastTarget = true;
            this.GetComponent<Image>().raycastTarget = false;
            countPriceColor = countPriceLicensePlate = countPriceHorn = countBuyItems = 0;
            EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, countBuyItems);
        }
        else
        {
            BuyPanelDescriptionText.text = "Not enough tokens to buy, do you want to buy more tokens?";
            garageManager.buyPanelBuyButton.onClick.RemoveAllListeners();
            garageManager.buyPanelBuyButton.onClick.AddListener(LoadScreenShopTokens);
            garageManager.statusBuyButton = GarageManager.StatusBuyButton.buyTokensCustomization;
        }
    }
    public void CheckCountTokensBuy()
    {
        if(countBuyItems > 0)
        {
            ChangeStatusBuyPanel("Open");
        }
    }
    public void ChangeStatusBuyPanel(string _anim)
    {
        garageManager.buyPanel.SetTrigger(_anim);
        if(garageManager.statusBuyButton == GarageManager.StatusBuyButton.buyTokensBuyCar)
        {
            LeftAndRightButtons.SetTrigger("Open");
            garageManager.statusBuyButton = GarageManager.StatusBuyButton.none;
            buyCar.SetTrigger("Open");
            shopButton.SetTrigger("Open");
        }
    }
    public void ChangeBuyButtonItems()
    {
        BuyPanelDescriptionText.text = "Are you sure?";
        garageManager.buyPanelBuyButton.onClick.RemoveAllListeners();
        garageManager.buyPanelBuyButton.onClick.AddListener(BuyItemsForCurrentCar);
    }
    public void ChangeBuyButtonCar()
    {
        buyCar.SetTrigger("Close");
        BuyPanelDescriptionText.text = "Are you sure?";
        garageManager.buyPanelBuyButton.onClick.RemoveAllListeners();
        garageManager.buyPanelBuyButton.onClick.AddListener(garageManager.BuyCar);
    }
    public void LoadScreenShopTokens()
    {
        ScreenManager.instance.ActiveNewScreen(0);
    }
    public void StatusButtonsBuyAndPlay()
    {
        if (!DataManager.instance.cars[currentCarIndex].statusCar)
        {
            if (customButton.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            {
                customButton.SetTrigger("Close");
            }
            if (!buyCar.GetCurrentAnimatorStateInfo(0).IsName("Open") && !backInShop)
            {
                buyCar.SetTrigger("Open");
            }
            if (garageManager.playThisCar.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            {
                garageManager.playThisCar.SetTrigger("Close");
            }
        }
        else
        {
            if (!customButton.GetCurrentAnimatorStateInfo(0).IsName("Open") && !backInShop)
            {
                customButton.SetTrigger("Open");
            }
            if (buyCar.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            {
                buyCar.SetTrigger("Close");
            }
            if (!garageManager.playThisCar.GetCurrentAnimatorStateInfo(0).IsName("Open") && !backInShop)
            {
                garageManager.playThisCar.SetTrigger("Open");
            }
        }
        backInShop = false;
    }
    public void ActiveCustoMizeManager()
    {
        if(DataManager.instance.cars[currentCarIndex].statusCar)
        {
            garageManager.playThisCar.SetTrigger("Close");
            //scrollStatus = false;
            carGarageHelpers[currentCarIndex].startCustomCurrentCar = true;
            lastColor = DataManager.instance.cars[currentCarIndex].currentColor;
            lastStatusSpoiler = DataManager.instance.cars[currentCarIndex].Spoiler;
            lastStatusDarkenedWindow = DataManager.instance.cars[currentCarIndex].darkenedWindow;
            lastStatusLicencePlate = DataManager.instance.currentLicensePlate;
            lastHorn = DataManager.instance.currentHorn;
            EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, 0);
            garageManager.GetComponent<Image>().raycastTarget = false;
            this.GetComponent<Image>().raycastTarget = true;
            Customize_window.SetTrigger("Open");
            customButton.SetTrigger("Close");
            shopButton.SetTrigger("Close");
            backToMenu.SetTrigger("Close");
            LeftAndRightButtons.SetTrigger("Close");
        }
    }
    public void PassiveCustoMizeManager()
    {
        carGarageHelpers[currentCarIndex].startCustomCurrentCar = false;
        countPriceColor = countPriceLicensePlate = countBuyItems = 0;
        currentCarIndexInMenu.currentCarObj.transform.GetChild(currentIndexColor).gameObject.SetActive(false);
        currentCarIndexInMenu.currentCarObj.transform.GetChild(lastColor).gameObject.SetActive(true);
        currentIndexColor = lastColor;
        
        if (lastStatusSpoiler)
        {
            spoilerSelected = lastStatusSpoiler;
            for (int i = 0; i < currentCarIndexInMenu.currentCarObj.transform.childCount; i++)
            {
                currentCarIndexInMenu.currentCarObj.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
            }
        }
        else
        {
            spoilerSelected = lastStatusSpoiler;
            for (int i = 0; i < currentCarIndexInMenu.currentCarObj.transform.childCount; i++)
            {
                currentCarIndexInMenu.currentCarObj.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
        }
        if (lastStatusDarkenedWindow)
        {
            darkenedWindowSelected = lastStatusDarkenedWindow;
            for (int i = 0; i < currentCarIndexInMenu.currentCarObj.transform.childCount; i++)
            {
                currentCarIndexInMenu.currentCarObj.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
                currentCarIndexInMenu.currentCarObj.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
        }
        else
        {
            darkenedWindowSelected = lastStatusDarkenedWindow;
            for (int i = 0; i < currentCarIndexInMenu.currentCarObj.transform.childCount; i++)
            {
                currentCarIndexInMenu.currentCarObj.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
                currentCarIndexInMenu.currentCarObj.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(false);
            }
        }
        licensePlate[currentIndexLicensePlate].SetActive(false);
        licensePlate[lastStatusLicencePlate].SetActive(true);
        currentIndexLicensePlate = lastStatusLicencePlate;
        garageManager.GetComponent<Image>().raycastTarget = true;
        this.GetComponent<Image>().raycastTarget = false;
        garageManager.playThisCar.SetTrigger("Open");
        Customize_window.SetTrigger("Close");
        customButton.SetTrigger("Open");
        shopButton.SetTrigger("Open");
        backToMenu.SetTrigger("Open");
        LeftAndRightButtons.SetTrigger("Open");
    }
    public void RightLicensePlate()
    {
        if(currentIndexLicensePlate < countLicensePlate - 1)
        {
            licensePlate[currentIndexLicensePlate].SetActive(false);
            currentIndexLicensePlate++;
            licensePlate[currentIndexLicensePlate].SetActive(true);
            if (!DataManager.instance.LicensePlate[currentIndexLicensePlate])
            {
                if (countPriceLicensePlate == 0)
                {
                    countPriceLicensePlate += 777;
                    countBuyItems += 777;
                }
            }
            else
            {
                if (countPriceLicensePlate > 0)
                {
                    countPriceLicensePlate -= 777;
                    countBuyItems -= 777;
                }
            }
            EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, countBuyItems);
        }
    }
    public void LeftLicensePlate()
    {
        if(currentIndexLicensePlate > 0)
        {
            licensePlate[currentIndexLicensePlate].SetActive(false);
            currentIndexLicensePlate--;
            licensePlate[currentIndexLicensePlate].SetActive(true);
            if(!DataManager.instance.LicensePlate[currentIndexLicensePlate])
            {
                if(countPriceLicensePlate == 0)
                {
                    countPriceLicensePlate += 777;
                    countBuyItems += 777;
                }
            }
            else
            {
                if(countPriceLicensePlate > 0)
                {
                    countPriceLicensePlate -= 777;
                    countBuyItems -= 777;
                }
            }
            EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, countBuyItems);
        }
    }
    public void ChooseDarkenedWindow()
    {
        if(!darkenedWindowSelected)
        {
            darkenedWindowSelected = true;
            for(int i = 0; i < currentCarIndexInMenu.currentCarObj.transform.childCount; i++)
            {
                currentCarIndexInMenu.currentCarObj.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
                currentCarIndexInMenu.currentCarObj.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
            if (!DataManager.instance.cars[currentCarIndex].darkenedWindow)
            {
                countBuyItems += 280;
            }
            EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, countBuyItems);
        }
    }
    public void RemoveDarkenedWindow()
    {
        if(darkenedWindowSelected)
        {
            darkenedWindowSelected = false;
            for (int i = 0; i < currentCarIndexInMenu.currentCarObj.transform.childCount; i++)
            {
                currentCarIndexInMenu.currentCarObj.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
                currentCarIndexInMenu.currentCarObj.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(false);
            }
            if(!DataManager.instance.cars[currentCarIndex].darkenedWindow)
            {
                countBuyItems -= 280;
            }
            EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, countBuyItems);
        }
    }
    public void ChooseSelectionSpoiler()
    {
        if(!spoilerSelected)
        {
            spoilerSelected = true;
            for(int i = 0; i < currentCarIndexInMenu.currentCarObj.transform.childCount; i++)
            {
                currentCarIndexInMenu.currentCarObj.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
            }
            if (!DataManager.instance.cars[currentCarIndex].Spoiler)
            {
                countBuyItems += 450;
            }
            EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, countBuyItems);
        }
    }
    public void RemoveSelectionSpoilder()
    {
        if(spoilerSelected)
        {
            spoilerSelected = false;
            for (int i = 0; i < currentCarIndexInMenu.currentCarObj.transform.childCount; i++)
            {
                currentCarIndexInMenu.currentCarObj.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
            if (!DataManager.instance.cars[currentCarIndex].Spoiler)
            {
                countBuyItems -= 450;
            }
            EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, countBuyItems);
        }
    }
    public void LeftColor()
    {
        if (currentIndexColor > 0)
        {
            if (currentCarIndexInMenu.currentCarObj.transform.GetChild(currentIndexColor).gameObject.activeSelf)
            {
                currentCarIndexInMenu.currentCarObj.transform.GetChild(currentIndexColor).gameObject.SetActive(false);
                currentIndexColor--;
                currentCarIndexInMenu.currentCarObj.transform.GetChild(currentIndexColor).gameObject.SetActive(true);
                if(!DataManager.instance.cars[currentCarIndex].Colors[currentIndexColor])
                {
                    if (countPriceColor == 0)
                    {
                        countPriceColor += 1000;
                        countBuyItems += 1000;
                    }
                }
                else
                {
                    if (countPriceColor > 0)
                    {
                        countPriceColor -= 1000;
                        countBuyItems -= 1000;
                    }
                }
                EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, countBuyItems);
            }
        }
    }
    public void RightColor()
    {
        if (currentIndexColor < countCustomizeColor - 1)
        {
            if (currentCarIndexInMenu.currentCarObj.transform.GetChild(currentIndexColor).gameObject.activeSelf)
            {
                currentCarIndexInMenu.currentCarObj.transform.GetChild(currentIndexColor).gameObject.SetActive(false);
                currentIndexColor++;

                currentCarIndexInMenu.currentCarObj.transform.GetChild(currentIndexColor).gameObject.SetActive(true);
                if (!DataManager.instance.cars[currentCarIndex].Colors[currentIndexColor])
                {
                    if(countPriceColor == 0)
                    {
                        countPriceColor += 1000;
                        countBuyItems += 1000;
                    }
                }
                else
                {
                    if (countPriceColor > 0)
                    {
                        countPriceColor -= 1000;
                        countBuyItems -= 1000;
                    }
                }
                EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, countBuyItems);
            }
        }
    }
    public void LeftHorn()
    {
        if(currentIndexHorn > 0)
        {
            currentIndexHorn--;
            SoundManager.instance.audioSource.PlayOneShot(SoundManager.instance.horns[currentIndexHorn], 0.6f);
            if (!DataManager.instance.cars[currentCarIndex].horns[currentIndexHorn])
            {
                if (countPriceHorn == 0)
                {
                    countPriceHorn += 220;
                    countBuyItems += 220;
                }
            }
            else
            {
                if (countPriceHorn > 0)
                {
                    countPriceHorn -= 220;
                    countBuyItems -= 220;
                }
            }
            EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, countBuyItems);
        }
    }
    public void RightHorn()
    {
        if(currentIndexHorn < SoundManager.instance.horns.Length - 1)
        {
            currentIndexHorn++;
            SoundManager.instance.audioSource.PlayOneShot(SoundManager.instance.horns[currentIndexHorn], 0.6f);
            if(!DataManager.instance.cars[currentCarIndex].horns[currentIndexHorn])
            {
                if (countPriceHorn == 0)
                {
                    countPriceHorn += 220;
                    countBuyItems += 220;
                }
            }
            else
            {
                if(countPriceHorn > 0)
                {
                    countPriceHorn -= 220;
                    countBuyItems -= 220;
                }
            }
            EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, countBuyItems);
        }
    }
}
