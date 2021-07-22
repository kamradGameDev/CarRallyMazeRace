using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
    }
    public event Action<int> CollectFlagsAction;
    public event Action<StatusCurrentLevel> StatusLevelAction;
    public event Action<char, int> CheckCountLivesAction;
    public event Action<int> PointsCurrentLevelAction;
    public event Action<int, int> CheckCountTokensAction;
    public void CheckCountTokens(int _newCountTokens, int _newCountPrice)
    {
        if(CheckCountTokensAction != null)
        {
            CheckCountTokensAction(_newCountTokens, _newCountPrice);
        }
    }
    public void NewPointsLevel(int _newPoints)
    {
        LevelManager.instance.countPoints += _newPoints;
        if(PointsCurrentLevelAction != null)
        {
            PointsCurrentLevelAction(_newPoints);
        }
    }
    public void CheckCountLives(char _char, int _newCountLives)
    {
        if (_char == '+')
        {
            SavedData.instance.livesCount += _newCountLives;
            
        }
        else if(_char == '-')
        {
            SavedData.instance.livesCount -= _newCountLives;
        }
        SavedData.instance.Saved();
        if (CheckCountLivesAction != null)
        {
            CheckCountLivesAction(_char, SavedData.instance.livesCount);
        }
    }
    public void StatusLevel(StatusCurrentLevel _statusCurrentLevel)
    {
        LevelManager.instance.statusCurrentLevel = _statusCurrentLevel;
        if (StatusLevelAction != null)
        {
            StatusLevelAction(_statusCurrentLevel);
        }
    }
    public void CollectFlags(int _flags)
    {
        LevelManager.instance.countCollectFlags += _flags;
        if (LevelManager.instance.countCollectFlags >= LevelManager.instance.maxCountFlagInLevel)
        {
            LevelManager.instance.OpenParking();
        }
        if (CollectFlagsAction != null)
        {
            CollectFlagsAction(_flags);
        }
    }
}
