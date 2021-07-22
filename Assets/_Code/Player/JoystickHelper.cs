using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickHelper : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public static JoystickHelper instance;
    public MovementCar movementCar;
    public Image joystickBG;
    public Image joystick;
    private Vector2 inputVector;
    public bool movementStatus;
    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(LevelManager.instance.statusCurrentLevel == StatusCurrentLevel.process)
        {
            Vector2 _pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBG.rectTransform, eventData.position, eventData.pressEventCamera, out _pos))
            {
                _pos.x = (_pos.x / joystickBG.rectTransform.sizeDelta.x);
                _pos.y = (_pos.y / joystickBG.rectTransform.sizeDelta.y);
            }
            inputVector = new Vector2(_pos.x * 3, _pos.y * 3);
            inputVector = (inputVector.sqrMagnitude > 1.0f * 1.0f) ? inputVector.normalized : inputVector;

            //joystick.rectTransform.anchoredPosition = new Vector2(inputVector.x * (joystickBG.rectTransform.sizeDelta.x / 3),
            //    inputVector.y * (joystickBG.rectTransform.sizeDelta.y / 3));
            if (Mathf.Abs(inputVector.x) > Mathf.Abs(inputVector.y))
            {
                if (inputVector.x > 0)
                {
                    //Debug.Log("right");
                    joystick.rectTransform.anchoredPosition = new Vector2(37, 0);
                }
                else
                {
                    //Debug.Log("left");
                    joystick.rectTransform.anchoredPosition = new Vector2(-37, 0);
                }
            }
            else
            {
                if (inputVector.y > 0)
                {
                    //Debug.Log("up");
                    joystick.rectTransform.anchoredPosition = new Vector2(0, 37);
                }
                if (inputVector.y < 0)
                {
                    //Debug.Log("down");
                    joystick.rectTransform.anchoredPosition = new Vector2(0, -37);
                }
            }
            movementStatus = true;
            movementCar.newDir = inputVector;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joystick.rectTransform.anchoredPosition = Vector2.zero;
    }
}
