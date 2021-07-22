using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name.Equals("EndLevel"))
        {
            EventManager.instance.StatusLevel(StatusCurrentLevel.win);
        }
        if(collision.CompareTag("Flag") && !collision.GetComponent<ItemCollect>().collisionPlayerCar)
        {
            EventManager.instance.CollectFlags(1);
            GameManager.newPoints += 45;
            StartCoroutine(NewPoints());
            StartCoroutine(collision.GetComponent<ItemCollect>().DissolveSprite(true));
        }
        if(collision.CompareTag("Rudder") || collision.CompareTag("RearViewMirror") || collision.CompareTag("Carburetor") && !collision.GetComponent<ItemCollect>().collisionPlayerCar)
        {
            GameManager.newPoints += 45;
            StartCoroutine(NewPoints());
            StartCoroutine(collision.GetComponent<ItemCollect>().DissolveSprite(true));
        }
        if ((collision.CompareTag("Fuel") || collision.CompareTag("Clock")) && !collision.GetComponent<ItemCollect>().collisionPlayerCar)
        {
            if(collision.CompareTag("Clock"))
            {
                LevelManager.instance.timeCount += 5f;
            }
            if(collision.CompareTag("Fuel") && !collision.GetComponent<ItemCollect>().collisionPlayerCar)
            {
                float _coefficient = LevelManager.instance.maxFuelCount / 100;
                LevelManager.instance.fuelCount += _coefficient * 5;
                if(LevelManager.instance.fuelCount > LevelManager.instance.maxFuelCount)
                {
                    LevelManager.instance.fuelCount = LevelManager.instance.maxFuelCount;
                }
            }
            GameManager.newPoints += 45;
            StartCoroutine(NewPoints());
            StartCoroutine(collision.GetComponent<ItemCollect>().DissolveSprite(true));
        }
        if (collision.CompareTag("Enemy"))
        {
            if(LevelManager.instance.statusCurrentLevel == StatusCurrentLevel.process)
            {
                DataManager.instance.cars[DataManager.instance.currentCar].fuelCount = LevelManager.instance.fuelCount;
                DataManager.instance.Saved();
                EventManager.instance.CheckCountLives('-', 1);
                if (SavedData.instance.livesCount > 0)
                {
                    AdManager.instance.ShowInterAd();
                }
                EventManager.instance.StatusLevel(StatusCurrentLevel.collsionEmemy);
            }
        }
    }
    private IEnumerator NewPoints()
    {
        yield return null;
        while(GameManager.newPoints > 0)
        {
            EventManager.instance.NewPointsLevel(5);
            GameManager.newPoints -= 5;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
