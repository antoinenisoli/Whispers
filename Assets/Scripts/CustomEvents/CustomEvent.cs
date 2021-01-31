using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomEvent : MonoBehaviour
{
    [SerializeField] protected bool playSound;
    [SerializeField] Transform soundLocalisation;
    [SerializeField] protected SoundEvent soundEvent;
    protected bool played;

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
            if (playSound && soundEvent)
            {
                if (soundEvent.onHold)
                    PlaySound();
            }
        }
    }

    public virtual void PlaySound()
    {
        StartCoroutine(ExecuteSoundEvent());
    }

    public IEnumerator ExecuteSoundEvent()
    {
        yield return new WaitForSeconds(soundEvent.delayAfterEvent);
        if (!(soundEvent.playOnce && played))
        {
            played = true;

            if (soundLocalisation != null)
                SoundManager.instance.PlayAudio(soundEvent.clip.name, soundLocalisation);
            else
                SoundManager.instance.PlayAudio(soundEvent.clip.name, transform);
        }
    }

    public virtual void DoEvent()
    {
        ready = true;
    }
}
