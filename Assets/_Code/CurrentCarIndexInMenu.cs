using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentCarIndexInMenu : MonoBehaviour
{
    public GameObject currentCarObj;
    public GarageManager garageManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentCarObj = collision.gameObject;
        CustomizationManager.instance.countCustomizeColor = currentCarObj.transform.childCount; 
        for(int i = 0; i < CustomizationManager.instance.countCustomizeColor; i++)
        {
            if(currentCarObj.transform.GetChild(i).gameObject.activeSelf)
            {
                CustomizationManager.instance.currentIndexColor = i;
                break;
            }
        }
        for(int i = 0; i < CustomizationManager.instance.cars.Length; i++)
        {
            if(CustomizationManager.instance.cars[i] == currentCarObj)
            {
                CustomizationManager.instance.currentCarIndex = i;
                CustomizationManager.instance.StatusButtonsBuyAndPlay();
                break;
            }
        }
    }
}
