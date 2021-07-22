using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickUIHelper : MonoBehaviour, IPointerDownHandler
{
    private Animator animatorClick;
    public int currentClickLevel;
    private void Start()
    {
        animatorClick = this.GetComponent<Animator>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        animatorClick.SetTrigger("Click");
        currentClickLevel = int.Parse(this.transform.GetChild(0).GetComponent<Text>().text);
        if(currentClickLevel == 1 && SavedData.instance.livesCount > 0)
        {
            LevelManager.instance.currentLevel = 0;
            GameManager.instance.currentChapter = 0;
            StartGameLevel();
            ScreenManager.instance.screenLoad.SetTrigger("Load");
        }
        else
        {
            if (SavedData.instance.livesCount > 0 && LevelStatusManager.instance.statusLevelComply[currentClickLevel - 2])
            {
                LevelManager.instance.currentLevel = currentClickLevel - 1;
                GameManager.instance.currentChapter = (currentClickLevel - 1) / 20;
                StartGameLevel();
                ScreenManager.instance.screenLoad.SetTrigger("Load");
            }
        }
    }

    private void StartGameLevel()
    {
        ScreenManager.instance.ActiveNewScreen(4);
        ScreenManager.instance.ActiveGameLevel(currentClickLevel - 1);
    }
}
