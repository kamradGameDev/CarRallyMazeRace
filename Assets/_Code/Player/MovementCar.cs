using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovementCar : MonoBehaviour
{
    public Vector2 newDir;
    private void Start()
    {
        mazeMover = GetComponent<MazeMover>();
    }

    MazeMover mazeMover;

    private void Update()
    {
        if(Vector2.Distance(this.transform.position, newDir) < 0.05f)
        {
            return;
        }
        if (Mathf.Abs(newDir.x) >= Mathf.Abs(newDir.y))
        {
            newDir.y = 0;
        }
        else
        {
            newDir.x = 0;
        }

        mazeMover.SetDesiredDirection(newDir.normalized, true);
    }

}
