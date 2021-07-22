using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject ButtonsMoreNullLives, ButtonsNullLives, ButtonsNullFuelCount;
    public SelectedLevelManager selectedLevelManager;
    public static int newPoints;
    public Animator gameOverPanel, pauseMenu;
    public Text countTokensText, countBuyItemsText;
    public int currentChapter;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
    public void StartGame()
    {
        EventManager.instance.CheckCountTokensAction += ChangingCountTokens;
        EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, CustomizationManager.instance.countBuyItems);
        EventManager.instance.CheckCountLives(' ', SavedData.instance.livesCount);
    }
    private void ChangingCountTokens(int _countTokens, int _countPrice)
    {
        countTokensText.text = _countTokens.ToString();
        countBuyItemsText.text = _countPrice.ToString();
    }
    public void PauseGame()
    {
        pauseMenu.SetTrigger("Open");
        EventManager.instance.StatusLevel(StatusCurrentLevel.pause);
    }
    public void ContinueGame()
    {
        if (pauseMenu.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
            EventManager.instance.StatusLevel(StatusCurrentLevel.process);
            pauseMenu.SetTrigger("Close");
        }
        else
        {
            pauseMenu.GetComponent<Image>().raycastTarget = false;
        }  
    }
}
