using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelComply : MonoBehaviour
{
    public Text pointsCount, tokensCount;
    public Animator animator;
    public Animator[] buttons;
    public GameObject points, tokens;
    private float rewardTokens;
    private bool loadMenu;
    public void StartLevel()
    {
        EventManager.instance.StatusLevelAction -= LevelStatus;
        EventManager.instance.StatusLevelAction += LevelStatus;
    }
    private void LevelStatus(StatusCurrentLevel _statusCurrentLevel)
    {
        if(_statusCurrentLevel == StatusCurrentLevel.win)
        {
            rewardTokens = LevelManager.instance.countPoints / 2.2f;
            SavedData.instance.tokensCount += (int)rewardTokens;
            SavedData.instance.Saved();
            CameraController.instance.enabled = false;
            DataManager.instance.cars[DataManager.instance.currentCar].fuelCount = LevelManager.instance.fuelCount;
            DataManager.instance.Saved();
            points.SetActive(true);
            tokens.SetActive(true);
            pointsCount.text = tokensCount.text = "";
            if(!LevelStatusManager.instance.statusLevelComply[LevelManager.instance.currentLevel])
            {
                //GameManager.instance.selectedLevelManager.StatusButtons(LevelManager.instance.currentLevel);
                LevelStatusManager.instance.statusLevelComply[LevelManager.instance.currentLevel] = true;
                LevelStatusManager.instance.Saved();
            }
            animator.SetTrigger("Open");
            StartCoroutine(EndLevel());
        }
        else if(_statusCurrentLevel == StatusCurrentLevel.nullTime)
        {
            DataManager.instance.cars[DataManager.instance.currentCar].fuelCount = LevelManager.instance.fuelCount;
            DataManager.instance.Saved();
            LevelManager.instance.EndLevel();
            GameManager.instance.ButtonsMoreNullLives.SetActive(true);
            GameManager.instance.ButtonsNullLives.SetActive(false);
            GameManager.instance.gameOverPanel.SetTrigger("Open");
        }
        else if(_statusCurrentLevel == StatusCurrentLevel.collsionEmemy)
        {
            EventManager.instance.StatusLevelAction -= LevelStatus;
            LevelManager.instance.countPoints = 0;
            StopCoroutine(EndLevel());
            DataManager.instance.cars[DataManager.instance.currentCar].fuelCount = LevelManager.instance.fuelCount;
            DataManager.instance.Saved();
            JoystickHelper.instance.movementStatus = false;
            LevelManager.instance.EndLevel();
            if (SavedData.instance.livesCount == 0)
            {
                GameManager.instance.ButtonsMoreNullLives.SetActive(false);
                GameManager.instance.ButtonsNullLives.SetActive(true);
                GameManager.instance.gameOverPanel.SetTrigger("Open");
            }
            else
            {
                ScreenManager.instance.screenLoad.SetTrigger("Load");
                LevelManager.instance.StartLevel();
                //restart level
            }
        }
        else if(_statusCurrentLevel == StatusCurrentLevel.nullFuel)
        {
            DataManager.instance.cars[DataManager.instance.currentCar].fuelCount = LevelManager.instance.fuelCount;
            DataManager.instance.Saved();
            JoystickHelper.instance.movementStatus = false;
            LevelManager.instance.EndLevel();
            GameManager.instance.ButtonsMoreNullLives.SetActive(false);
            GameManager.instance.ButtonsNullLives.SetActive(false);
            GameManager.instance.ButtonsNullFuelCount.SetActive(true);
            GameManager.instance.gameOverPanel.SetTrigger("Open");
        }
    }
    private IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(1f);
        int _countpoints = LevelManager.instance.countPoints;
        float _countTokens = 0f;
        
        while(_countpoints > 0)
        {
            yield return new WaitForSeconds(0.01f);
            
            if (_countpoints > 0)
            {
                _countpoints -= 10;
                pointsCount.text = _countpoints.ToString();
                
            }
            if(_countTokens < rewardTokens)
            {
                _countTokens += 5;
                tokensCount.text = Mathf.Round(_countTokens).ToString();
            }
            else
            {
                _countpoints = 0;
                
                pointsCount.text = _countpoints.ToString();
                tokensCount.text = Mathf.Round(_countTokens).ToString();
                EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, 0);
                Invoke("PassiveTexts", 1.5f);
                Invoke("OpenButtons", 0.5f);
            }
        }
    }
    public void MainMenu()
    {
        EventManager.instance.StatusLevelAction += LevelStatus;
        LevelManager.instance.countPoints = 0;
        animator.SetTrigger("Close");
        loadMenu = true;
        StopCoroutine(EndLevel());
        buttons[0].SetTrigger("Close");
        buttons[1].SetTrigger("Close");
        rewardTokens = 0;
    }
    private void LoadScreenMenuLevels()
    {
        if(loadMenu)
        {
            loadMenu = false;
            ScreenManager.instance.ActiveNewScreen(3);
            ScreenManager.instance.screenLoad.SetTrigger("Load");
            EventManager.instance.StatusLevelAction -= LevelStatus;
        }
       
    }
    private void OpenButtons()
    {
        if(LevelManager.instance.currentLevel < LevelStatusManager.instance.statusLevelComply.Length - 1)
        {
            buttons[0].SetTrigger("Open");
        }
        else
        {
            buttons[0].SetTrigger("NoNextLevel");
        }
        buttons[1].SetTrigger("Open");
    }
    private void PassiveTexts()
    {
        points.SetActive(false);
        tokens.SetActive(false);
    }
    public void StartNextLevel()
    {
        rewardTokens = 0;
        ScreenManager.instance.screenLoad.SetTrigger("Load");
        buttons[0].SetTrigger("Close");
        buttons[1].SetTrigger("Close");
        StopCoroutine(EndLevel());
        animator.SetTrigger("Close");
        LevelManager.instance.currentLevel += 1;
        ScreenManager.instance.ActiveGameLevel(LevelManager.instance.currentLevel);
    }
}
