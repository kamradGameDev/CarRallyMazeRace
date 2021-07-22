using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GarageManager : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public CustomWindow customWindow;
    public enum StatusBuyButton
    {
        none, buyTokensCustomization, buyTokensBuyCar
    }
    public StatusBuyButton statusBuyButton;
    public Button startLevelButton;
    public Text fuelCountCurrentCar;
    public Transform fuelArrow;
    public float targetRotateArrow;
    public Text carPriceText;
    public Button buyPanelBuyButton;
    public Animator playThisCar, BuyMoreFuel, buyPanel;
    public Transform parentSwipe;
    public Vector3 target;
    public float speed = 20f;
    public bool nextMove = true;
    public int maxCountSwipe;
    public int currentCountSwipe;
    public float distanceXScroll;
    public string[] carPrice;
    public Vector3 defaultRotate;
    public int countPriceCar;
    [SerializeField]private Transform[] hearts;

    private void Start()
    {
        defaultRotate = fuelArrow.transform.eulerAngles;
        maxCountSwipe = parentSwipe.childCount;
        distanceXScroll = parentSwipe.GetChild(1).position.x - parentSwipe.GetChild(0).position.x;
        playThisCar.SetTrigger("Open");
        StartCoroutine(SmoothRotateArrow());
    }
    public void OpenGarage()
    {
        EventManager.instance.CheckCountLivesAction += CheckCountLives;
        EventManager.instance.CheckCountLives(' ', SavedData.instance.livesCount);
        AllStatusForCurrentCar();
    }
    public void ExitFromTheGarage()
    {
        EventManager.instance.CheckCountLivesAction -= CheckCountLives;
    }
    private void CheckCountLives(char _char, int _newCount)
    {
        for (var i = 0; i < 6; i++)
        {
            if (i > SavedData.instance.maxLivesCount - 1)
            {
                hearts[i].gameObject.SetActive(false);
            }
            else
            {
                hearts[i].gameObject.SetActive(true);
            }
        }
        for (var i = 0; i < hearts.Length; i++)
        {
            if(i < _newCount)
            {
                hearts[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                hearts[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
    public void AllStatusForCurrentCar()
    {
        targetRotateArrow = (DataManager.instance.cars[currentCountSwipe].fuelCount / DataManager.instance.cars[currentCountSwipe].maxCountFuel);
        float _coefficient = (DataManager.instance.cars[currentCountSwipe].fuelCount / DataManager.instance.cars[currentCountSwipe].maxCountFuel) * 100;
        targetRotateArrow = _coefficient * 1.6f;
        targetRotateArrow *= -1;
        fuelCountCurrentCar.text = Mathf.RoundToInt(_coefficient).ToString() + "%";
    }
    public void BuyCar()
    {
        carPrice[currentCountSwipe] = carPrice[currentCountSwipe].Replace(" ", string.Empty);
        countPriceCar = int.Parse(carPrice[currentCountSwipe]);
        CustomizationManager.instance.buyCar.SetTrigger("Close");
        CustomizationManager.instance.LeftAndRightButtons.SetTrigger("Close");
        CustomizationManager.instance.garageManager.buyPanelBuyButton.onClick.RemoveAllListeners();
        statusBuyButton = StatusBuyButton.buyTokensBuyCar;
        buyPanel.SetTrigger("Open");
        CustomizationManager.instance.shopButton.SetTrigger("Close");
        if (SavedData.instance.tokensCount >= countPriceCar)
        {
            CustomizationManager.instance.BuyPanelDescriptionText.text = "Are you sure?";
            CustomizationManager.instance.garageManager.buyPanelBuyButton.onClick.AddListener(BuyCarShure);
        }
        else
        {
            CustomizationManager.instance.BuyPanelDescriptionText.text = "Not enough tokens to buy, do you want to buy more tokens?";
            CustomizationManager.instance.garageManager.buyPanelBuyButton.onClick.AddListener(CustomizationManager.instance.LoadScreenShopTokens);
        }
    }
    public void BuyCarShure()
    {
        buyPanel.SetTrigger("Close");
        //CustomizationManager.instance.buyCar.gameObject.SetActive(false);
        //CustomizationManager.instance.shopButton.SetTrigger("Open");
        //CustomizationManager.instance.LeftAndRightButtons.SetTrigger("Open");
        statusBuyButton = StatusBuyButton.none;
        carPrice[currentCountSwipe] = carPrice[currentCountSwipe].Replace(" ", string.Empty);
        int _price = int.Parse(carPrice[currentCountSwipe]);
        SavedData.instance.tokensCount -= _price;
        EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, CustomizationManager.instance.countBuyItems);
        SavedData.instance.Saved();
        DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].statusCar = true;
        DataManager.instance.Saved();
        CustomizationManager.instance.StatusButtonsBuyAndPlay();
    }
    public void ClosePanelStartGameLevel()
    {
        BuyMoreFuel.SetTrigger("Close");
        StartCoroutine(TimeLoadGameLevel(false));
    }
    public void CheckCarStatusAndLoadNextScreen()
    {
        BuyMoreFuel.SetTrigger("Open");
        BuyMoreFuel.GetComponent<Image>().raycastTarget = true;
        CheckCurrectCarFuelCount();
    }

    public void CheckCurrectCarFuelCount()
    {
        //float _coefficient = DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].fuelCount /
                           //DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].maxCountFuel;
        //float _checkPercentFuel = 100 / _coefficient;
        startLevelButton.interactable = DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].fuelCount > 0;
    }
    public void StartGameLevel()
    {
        BuyMoreFuel.SetTrigger("Close");
        StartCoroutine(TimeLoadGameLevel(true));
    }
    private IEnumerator TimeLoadGameLevel(bool _loadGameLevel)
    {
        yield return new WaitForSeconds(1f);
        if(_loadGameLevel)
        {
            BuyMoreFuel.GetComponent<Image>().raycastTarget = false;
            DataManager.instance.currentCar = CustomizationManager.instance.currentCarIndex;
            DataManager.instance.Saved();
            ScreenManager.instance.ActiveNewScreen(3);
        }
        else
        {
            BuyMoreFuel.GetComponent<Image>().raycastTarget = false;
        }
    }
    public void BuyMoreFuelForCurrentCar()
    {
        if(DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].fuelCount == DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].maxCountFuel)
        {
            return;
        }
        if (SavedData.instance.tokensCount >= 250)
        {
            SavedData.instance.tokensCount -= 250;
            float _coefficient_0 = DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].maxCountFuel / 100f;
            float _coefficient_1 = _coefficient_0 * 25f;
            DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].fuelCount += _coefficient_1;
            if(DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].fuelCount > DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].maxCountFuel)
            {
                DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].fuelCount = DataManager.instance.cars[CustomizationManager.instance.currentCarIndex].maxCountFuel;
            }
            AllStatusForCurrentCar();
            EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, 0);
            SavedData.instance.Saved();
            DataManager.instance.Saved();
        }
    }
    public void StartSmoothMove()
    {
        StartCoroutine(SmoothMove());
        StartCoroutine(customWindow.SmoothMove());
    }
    public void RightScroll()
    {
        if (nextMove && currentCountSwipe < maxCountSwipe - 1 && statusBuyButton == StatusBuyButton.none)
        {
            //startTime = Time.time;
            defaultRotate = fuelArrow.transform.eulerAngles;
            
            CustomizationManager.instance.scrollStatus = true;
            currentCountSwipe++;     
            
            if (!DataManager.instance.cars[currentCountSwipe].statusCar)
            {
                string _str = carPrice[currentCountSwipe];
                carPriceText.text = _str;
            }
            target = new Vector3(parentSwipe.position.x - distanceXScroll, parentSwipe.position.y, parentSwipe.position.z);
            AllStatusForCurrentCar();
        }
    }
    public void LeftScroll()
    {
        if (nextMove && currentCountSwipe > 0 && statusBuyButton == StatusBuyButton.none)
        {
            //startTime = Time.time;
            defaultRotate = fuelArrow.transform.eulerAngles;
            
            CustomizationManager.instance.scrollStatus = true;
            currentCountSwipe--;
            
            if (!DataManager.instance.cars[currentCountSwipe].statusCar)
            {
                string _str = carPrice[currentCountSwipe];
                carPriceText.text = _str;
            }
            target = new Vector3(parentSwipe.position.x + distanceXScroll, parentSwipe.position.y, parentSwipe.position.z);
            AllStatusForCurrentCar();
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(nextMove)
        {
            if(Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y))
            {
                if(eventData.delta.x > 0)
                {
                    RightScroll();
                }
                else
                {
                    LeftScroll();
                }
            }
        }
    }
    private IEnumerator SmoothRotateArrow()
    {
        while (true)
        {
            yield return null;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0,0, targetRotateArrow));
            fuelArrow.rotation = Quaternion.RotateTowards(fuelArrow.rotation, targetRotation, 180 * Time.deltaTime);
        }
    }
    private IEnumerator SmoothMove()
    {
        while(true)
        {
            yield return null;
            if(parentSwipe.position != target)
            {
                nextMove = false;
                parentSwipe.position = Vector2.MoveTowards(parentSwipe.position, target, speed * Time.deltaTime);
            }
            else
            {
                nextMove = true;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
