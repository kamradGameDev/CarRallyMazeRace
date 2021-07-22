using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeMover : MonoBehaviour
{
    private float tileDistanceTolerance = 0.001f; 

    public float Speed = 3;

    public Vector2 desiredDirection;
    public Vector2 targetPos;
    private Vector2 currentPos, velocity;
    private void Start()
    {
        currentPos = targetPos = this.transform.position;
    }
    private void Update()
    {
        UpdateTargetPosition();
        MoveToTargetPosition();
        MeasingTheSpeedOfTheMachine();
    }

    private void MeasingTheSpeedOfTheMachine()
    {
        velocity = ((Vector2)this.transform.position - currentPos) / Time.deltaTime;
        if (velocity != new Vector2(0, 0)) { SoundManager.instance.clips[0].volume = 0.5f; }
        else { SoundManager.instance.clips[0].volume = 0.2f; }
        currentPos = this.transform.position;
    }

    private void UpdateTargetPosition(bool force = false)
    {
        if (!force)
        {
            float distanceToTarget = Vector2.Distance(this.transform.position, targetPos);
            if (distanceToTarget > 0)
            {
                return;
            }
            else
            {
                if (desiredDirection.x > 0)
                {
                    this.transform.eulerAngles = new Vector3(0, 0, -90);
                }
                else if (desiredDirection.x < 0)
                {
                    this.transform.eulerAngles = new Vector3(0, 0, 90);
                }
                else if (desiredDirection.y > 0)
                {
                    this.transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else if (desiredDirection.y < 0)
                {
                    this.transform.eulerAngles = new Vector3(0, 0, 180);
                }
            }
        }

        targetPos += desiredDirection;

        targetPos = FloorPosition(targetPos);

        if (IsRoadTileEmpty(targetPos))
        {
            return;
        }

        targetPos = this.transform.position;
    }

    private Vector2 FloorPosition(Vector2 pos)
    {
        return new Vector2(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
    }

    private bool IsRoadTileEmpty(Vector2 pos)
    {
        if(LevelManager.instance.statusCurrentLevel == StatusCurrentLevel.process)
        {
            return GetTileAt(pos) == null;
        }
        return false;
    }

    private TileBase GetTileAt(Vector2 pos)
    {
        if(LevelManager.WallTilemap)
        {
            Vector3Int cellPos = LevelManager.WallTilemap.WorldToCell(pos);

            return LevelManager.WallTilemap.GetTile(cellPos);
        }
        return null;
    }

    private void MoveToTargetPosition()
    {
        if(Vector2.Distance(this.transform.position, targetPos) > 0f && JoystickHelper.instance.movementStatus)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetPos, Speed * Time.deltaTime);
            return;
        }
        else
        {
            JoystickHelper.instance.movementStatus = false;
        }

        if (Vector2.Distance(targetPos, this.transform.position) < tileDistanceTolerance)
        {
            this.transform.position = targetPos;
        }
    }

    public void SetDesiredDirection(Vector2 _newDir, bool canInstantUpdate = false)
    {
        Vector2 _nextPos = targetPos + _newDir;
        if (!IsRoadTileEmpty(_nextPos))
        {
            return;
        }
        desiredDirection = _newDir;


        if (canInstantUpdate && Vector2.Dot(desiredDirection, _newDir) < 0)
        {
            UpdateTargetPosition(true);
        }
    }
}
