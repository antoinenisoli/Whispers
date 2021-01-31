using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundEvent
{
    public bool playSound;
    public float delayAfterEvent;
    public AudioClip clip;
    public Transform soundLocalisation;
}

public abstract class CustomEvent : MonoBehaviour
{
    [SerializeField] protected SoundEvent soundEvent;
    protected bool done;
    protected bool ready;
    protected Camera viewCam;

    public virtual void Awake()
    {
        viewCam = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        FPS_Controller character = other.GetComponent<FPS_Controller>();
        if (character && !ready)
        {
            DoEvent();
        }
    }

    public virtual void DoEvent()
    {
        ready = true;
    }
}
