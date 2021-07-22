using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform currentPlayer;
    public float MinY, MaxY, MinX, MaxX;//, posZ;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
    private void LateUpdate()
    {
        this.transform.position = new Vector2
        (
            Mathf.Clamp(currentPlayer.transform.position.x, MinX, MaxX),
            Mathf.Clamp(currentPlayer.transform.position.y, MinY, MaxY)
            //Mathf.Clamp(this.transform.position.z, posZ, posZ)
        );
    }
}
