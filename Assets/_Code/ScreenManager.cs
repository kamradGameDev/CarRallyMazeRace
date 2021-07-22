using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;
    public Animator screenLoad;
    public GameObject[] screens;
    public GameObject[] levelsObj;
    public int nextScreen;
    public int lastScreen;
    public GarageManager garageManager;
    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
    }
    public void ActiveNewScreen(int _screenNumber)
    {
        nextScreen = _screenNumber;
        if (nextScreen == 4)
        {
            screenLoad.speed = 0.7f;
            SoundManager.instance.clips[0].Play();
        }
        else if(nextScreen == 3)
        {
            AdManager.instance.countAd = 0;
            GameManager.instance.selectedLevelManager.CheckLastFinishLevel();
            GameManager.instance.selectedLevelManager.StatusButtons(GameManager.instance.selectedLevelManager.lastFinishLevel);
            LevelManager.instance.ClearWallPropsInScene();
            LevelManager.instance.ClearItems();
            EventManager.instance.CheckCountLives(' ', SavedData.instance.livesCount);
            if (SoundManager.instance.clips[0].isPlaying) { SoundManager.instance.clips[0].Stop(); }
        }
        else if(nextScreen == 2)
        {
            garageManager.OpenGarage();
        }
        else
        {
            LevelManager.instance.ClearWallPropsInScene();
            LevelManager.instance.ClearItems();
            screenLoad.speed = 1.3f;
            if (SoundManager.instance.clips[0].isPlaying) { SoundManager.instance.clips[0].Stop(); }
        }
        screenLoad.SetTrigger("Load");
    }
    public void BackInShop()
    {
        screenLoad.SetTrigger("Load");
        Invoke("LoadBackToLastScreen", 0.3f);
    }
    private void LoadBackToLastScreen()
    {
        for (int i = 0; i < screens.Length; i++)
        {
            if (i == lastScreen)
            {
                screens[i].SetActive(true);
                if (garageManager.transform.parent.parent.gameObject.activeSelf)
                {
                    garageManager.StartSmoothMove();
                }
            }
            else
            {
                screens[i].SetActive(false);
            }
        }
        if(garageManager.statusBuyButton == GarageManager.StatusBuyButton.buyTokensCustomization)
        {
            CustomizationManager.instance.backInShop = true;
            CustomizationManager.instance.Customize_window.SetTrigger("Open");
            if(CustomizationManager.instance.countBuyItems <= SavedData.instance.tokensCount)
            {
                CustomizationManager.instance.BuyPanelDescriptionText.text = "Are you sure?";
                garageManager.buyPanelBuyButton.onClick.RemoveAllListeners();
                garageManager.buyPanelBuyButton.onClick.AddListener(CustomizationManager.instance.BuyItemsForCurrentCar);
            }
            //CustomizationManager.instance.customButton.SetTrigger("Open");
            //CustomizationManager.instance.shopButton.SetTrigger("Close");
            //CustomizationManager.instance.backToMenu.SetTrigger("Close");
            //CustomizationManager.instance.LeftAndRightButtons.SetTrigger("Close");
            //garageManager.playThisCar.SetTrigger("Close");
            garageManager.statusBuyButton = GarageManager.StatusBuyButton.none;
        }
        else if(garageManager.statusBuyButton == GarageManager.StatusBuyButton.buyTokensBuyCar)
        {
            CustomizationManager.instance.backInShop = true;
            if(CustomizationManager.instance.garageManager.countPriceCar <= SavedData.instance.tokensCount)
            {
                CustomizationManager.instance.BuyPanelDescriptionText.text = "Are you sure?";
                CustomizationManager.instance.garageManager.buyPanelBuyButton.onClick.AddListener(CustomizationManager.instance.garageManager.BuyCarShure);
            }
            //ustomizationManager.instance.Customize_window.SetTrigger("Close");
            //CustomizationManager.instance.customButton.SetTrigger("Close");
            //CustomizationManager.instance.shopButton.SetTrigger("Close");
            //CustomizationManager.instance.backToMenu.SetTrigger("Close");
            //CustomizationManager.instance.LeftAndRightButtons.SetTrigger("Close");
            //CustomizationManager.instance.buyCar.gameObject.SetActive(true);
            //CustomizationManager.instance.buyCar.SetTrigger("Close");
        }
    }
    public void ActiveGameLevel(int _numberLevel)
    {
        for (int i = 0; i < levelsObj.Length; i++)
        {
            if(i == _numberLevel)
            {
                levelsObj[i].SetActive(true);
                LevelManager.instance.activeLevel = levelsObj[i];
                LevelManager.instance.levelHelper = levelsObj[i].GetComponent<LevelHelper>();
            }
            else
            {
                levelsObj[i].SetActive(false);
            }
        }
    }
    private void ActiveNextScren()
    {
        for (int i = 0; i < screens.Length; i++)
        {
            if (i == nextScreen)
            {
                screens[i].SetActive(true);
                if (garageManager.transform.parent.parent.gameObject.activeSelf)
                {
                    garageManager.StartSmoothMove();
                }
                if (screens[i].name == "GameLevel")
                {
                    SoundManager.instance.clips[1].Stop();
                    SoundManager.instance.clips[0].Play();
                }
                else
                {
                    if(!SoundManager.instance.clips[1].isPlaying)
                    {
                        SoundManager.instance.clips[1].Play();
                    }
                    SoundManager.instance.clips[0].Stop();
                    CameraController.instance.enabled = false;
                    CameraController.instance.transform.position = new Vector3(0,0, -100);
                }
            }
            else
            {
                if (screens[i].activeSelf && !screens[i].name.Equals("Store"))
                {
                    lastScreen = i;
                }
                screens[i].SetActive(false);
            }
        }
    }
    private void EndStartLevel()
    {
        if (nextScreen == 4)
        {
            EventManager.instance.StatusLevel(StatusCurrentLevel.process);
        }
        else
        {
            LevelManager.instance.EndLevel();
            EventManager.instance.StatusLevel(StatusCurrentLevel.menu);
        }
    }
    private void TimeStartLevel()
    {
        if(nextScreen != 4)
        {
            CameraController.instance.enabled = false;
            return;
        }
        GameManager.instance.gameOverPanel.gameObject.GetComponent<Image>().raycastTarget = false;
        LevelManager.instance.player = LevelManager.instance.levelHelper.carPlayer.transform;
        CameraController.instance.transform.position = LevelManager.instance.player.position;
        LevelManager.instance.player.position = LevelManager.instance.levelHelper.playerStartPos;

        JoystickHelper.instance.movementCar = LevelManager.instance.levelHelper.movementCar;
        LevelManager.instance.levelHelper.mazeMover.targetPos = LevelManager.instance.levelHelper.playerStartPos;
        LevelManager.instance.levelHelper.movementCar.newDir = LevelManager.instance.levelHelper.playerStartPos;
        LevelManager.instance.levelHelper.mazeMover.SetDesiredDirection(LevelManager.instance.levelHelper.playerStartPos, false);
        LevelManager.instance.StartLevel();
        GameManager.instance.ContinueGame();
        
        for (int i = 0; i < LevelManager.instance.player.childCount; i++)
        {
            if(i == DataManager.instance.currentCar)
            {
                LevelManager.instance.player.GetChild(i).gameObject.SetActive(true);
                LevelManager.instance.currentPlayer = i;
            }
            else
            {
                LevelManager.instance.player.GetChild(i).gameObject.SetActive(false);
            }
        }
        for(int i = 0; i < LevelManager.instance.player.GetChild(LevelManager.instance.currentPlayer).childCount; i++)
        {
            if(i == DataManager.instance.cars[LevelManager.instance.currentPlayer].currentColor)
            {
                LevelManager.instance.player.GetChild(LevelManager.instance.currentPlayer).GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                LevelManager.instance.player.GetChild(LevelManager.instance.currentPlayer).GetChild(i).gameObject.SetActive(false);
            }
            if (DataManager.instance.cars[LevelManager.instance.currentPlayer].Spoiler)
            {
                LevelManager.instance.player.GetChild(LevelManager.instance.currentPlayer).GetChild(i).GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                LevelManager.instance.player.GetChild(LevelManager.instance.currentPlayer).GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
            if(DataManager.instance.cars[LevelManager.instance.currentPlayer].darkenedWindow)
            {
                LevelManager.instance.player.GetChild(LevelManager.instance.currentPlayer).GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
                LevelManager.instance.player.GetChild(LevelManager.instance.currentPlayer).GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                LevelManager.instance.player.GetChild(LevelManager.instance.currentPlayer).GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
                LevelManager.instance.player.GetChild(LevelManager.instance.currentPlayer).GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(false);
            }
        }
    }
}
