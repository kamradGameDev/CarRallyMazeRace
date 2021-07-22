using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHelper : MonoBehaviour
{   
    public Vector3[] roadBlockTiles;
    public Vector3 blockTile;
    public GameObject barrier;
    public EnemyMotion[] enemyMotions;
    public Transform carPlayer;
    public MazeMover mazeMover;
    public MovementCar movementCar;
    public Vector3 playerStartPos = new Vector3(1, 0, 0);
}
