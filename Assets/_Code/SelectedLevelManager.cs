using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedLevelManager : MonoBehaviour
{
    public Transform[] currentNumberTextLevels;
    public GameObject countLives, infinityLives;
    public int lastFinishLevel;
    [SerializeField] private Transform[] hearts;
    public void StartGame()
    {
        int _currentLevels = 0;
        for (int i = 0; i < currentNumberTextLevels.Length; i++)
        {
            currentNumberTextLevels[i].GetChild(0).GetComponent<Text>().text = (_currentLevels += 1).ToString();
        }
        EventManager.instance.CheckCountLivesAction += CheckNewCountLives;
    }
    public void CheckLastFinishLevel()
    {
        for(int i = 0; i < LevelStatusManager.instance.statusLevelComply.Length; i++)
        {
            if(LevelStatusManager.instance.statusLevelComply[i])
            {
                lastFinishLevel = i;
            }
        }
    }
    public void StatusButtons(int _currentLevel)
    {
        if(_currentLevel > 0)
        {
            for (int i = 0; i < currentNumberTextLevels.Length; i++)
            {
                if (i <= _currentLevel)
                {
                    currentNumberTextLevels[i].GetComponent<Image>().color = Color.white;
                    currentNumberTextLevels[i].transform.GetChild(0).GetComponent<Text>().color = Color.white;
                }
                if (i == _currentLevel + 1)
                {
                    currentNumberTextLevels[i].GetComponent<Image>().color = Color.white;
                    currentNumberTextLevels[i].transform.GetChild(0).GetComponent<Text>().color = Color.gray;
                }
            }
        }
        else
        {
            if (LevelStatusManager.instance.statusLevelComply[0])
            {
                currentNumberTextLevels[0].GetComponent<Image>().color = Color.white;
                currentNumberTextLevels[0].transform.GetChild(0).GetComponent<Text>().color = Color.white;
                currentNumberTextLevels[1].GetComponent<Image>().color = Color.white;
                currentNumberTextLevels[1].transform.GetChild(0).GetComponent<Text>().color = Color.gray;
            }
            else
            {
                currentNumberTextLevels[0].GetComponent<Image>().color = Color.white;
                currentNumberTextLevels[0].transform.GetChild(0).GetComponent<Text>().color = Color.gray;
            }
        }
        
    }
    private void CheckNewCountLives(char _char, int _newCount)
    {
        if(!SavedData.instance.unlimitedLives)
        {
            infinityLives.SetActive(false);
            countLives.gameObject.SetActive(true);
            for (int i = 0; i < 6; i++)
            {
                if (i > SavedData.instance.maxLivesCount - 1)
                {
                    hearts[i].gameObject.SetActive(false);
                }
                else
                {
                    hearts[i].gameObject.SetActive(true);
                }
            }
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < _newCount)
                {
                    hearts[i].transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    hearts[i].transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            countLives.SetActive(false);
            infinityLives.SetActive(true);
        }
    }
}