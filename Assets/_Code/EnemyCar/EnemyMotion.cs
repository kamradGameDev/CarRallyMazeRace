using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMotion : MonoBehaviour
{
    private Vector2 directionMove;
    public float speed = 2;
    public float dissolveAmount;
    public Vector2 startPos;
    private bool levelStatusProcess = false;
    public Transform[] targets;
    public int indexTarget;
    private void Start()
    {
        EventManager.instance.StatusLevelAction += ChangeLevelStatus;
    }
    private void ChangeLevelStatus(StatusCurrentLevel _statusCurrentLevel)
    {
        if(_statusCurrentLevel == StatusCurrentLevel.nullTime || _statusCurrentLevel == StatusCurrentLevel.collsionEmemy)
        {
            this.gameObject.SetActive(false);
        }
        if(_statusCurrentLevel == StatusCurrentLevel.process)
        {
            levelStatusProcess = true;
        }
        else
        {
            levelStatusProcess = false; 
            this.gameObject.SetActive(true);
        }
    }
    private void Update()
    {
        if(levelStatusProcess)
        {
            if(Vector2.Distance(this.transform.position, targets[indexTarget].position) > 0f)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, targets[indexTarget].position, speed * Time.deltaTime);
            }
            else
            {
                if (indexTarget < targets.Length - 1) { indexTarget++; }
                else { indexTarget = 0; }
                directionMove = targets[indexTarget].position - this.transform.position;
                this.transform.up = Vector2.MoveTowards(this.transform.up, directionMove, 180);
            }
        }
    }
}
