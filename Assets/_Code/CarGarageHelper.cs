using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGarageHelper : MonoBehaviour
{
    public bool startCustomCurrentCar;
    public float countFuel, maxCountFuel;
    public Vector2 startCustomPosCurrentCar, lastPosCurrentCar;
    private void Start()
    {
        lastPosCurrentCar = new Vector2(this.transform.localPosition.x, 0);
        startCustomPosCurrentCar = lastPosCurrentCar + new Vector2(1.5f, 0);
    }
    private void Update()
    {
        if(!CustomizationManager.instance.scrollStatus)
        {
            if (startCustomCurrentCar)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, startCustomPosCurrentCar, 4 * Time.deltaTime);
            }
            else
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, lastPosCurrentCar, 4 * Time.deltaTime);
            }
        }
    }
}
