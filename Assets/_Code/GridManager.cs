using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width, height;
    public int[,] gridArray;
    public GridManager(int width, int height)
    {
        this.width = width;
        this.height = height;

        gridArray = new int[width, height];

        for(int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int y = 0; y < gridArray.GetLength(1); y++)
            {
                //Utils
            }
        }
    }
}
public class InitializeGridManager
{
    GridManager gridManager = new GridManager(20,20);
}
