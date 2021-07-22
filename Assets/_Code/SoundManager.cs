using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource[] clips;
    public AudioSource audioSource;
    public AudioClip[] horns;
    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
    }
}
