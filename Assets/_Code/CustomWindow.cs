using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomWindow : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public Animator UpArrow, DownArrow;
    public Vector3 target;
    public float speedMove = 5f;
    public bool nextMove = false;
    public int maxCountSwipe;
    public int currentCountSwipe;
    public Transform parentSwipe;
    public float disatnceYScroll;
    private void Start()
    {
        //maxCountSwipe = parentSwipe.transform.childCount;
        disatnceYScroll = parentSwipe.GetChild(1).localPosition.y - parentSwipe.GetChild(0).localPosition.y;
        UpArrow.SetBool("Active", false);
        DownArrow.SetBool("Active", true);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(nextMove)
        {
            if(Mathf.Abs(eventData.delta.y) > Mathf.Abs(eventData.delta.x))
            {
                if(eventData.delta.y > 0)
                {
                    //Debug.Log("UpScroll");
                    UpScroll();
                }
                else
                {
                    //Debug.Log("DownScroll");
                    DownScroll();
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
       
    }
    private void DownScroll()
    {
        if (nextMove && currentCountSwipe > 0)
        {
            UpArrow.SetBool("Active", false);
            DownArrow.SetBool("Active", true);
            currentCountSwipe--;
            target = new Vector3(parentSwipe.localPosition.x, parentSwipe.transform.localPosition.y + disatnceYScroll, parentSwipe.localPosition.z);
        }
    }
    private void UpScroll()
    {
        if(nextMove && currentCountSwipe < maxCountSwipe - 1)
        {
            UpArrow.SetBool("Active", true);
            DownArrow.SetBool("Active", false);
            currentCountSwipe++;
            target = new Vector3(parentSwipe.localPosition.x, parentSwipe.transform.localPosition.y - disatnceYScroll, parentSwipe.localPosition.z);
        }
    }
    public  IEnumerator SmoothMove()
    {
        while (true)
        {
            yield return null;
            if(parentSwipe.transform.localPosition != target)
            {
                nextMove = false;
                parentSwipe.transform.localPosition = Vector2.MoveTowards(parentSwipe.transform.localPosition, target, speedMove * Time.deltaTime);
            }
            else
            {
                nextMove = true;
            }
        }
    }
}
