using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomEvent : MonoBehaviour
{
    [SerializeField] protected bool playSound;
    [SerializeField] Transform soundLocalisation;
    [SerializeField] protected SoundEvent soundEvent;
    protected bool soundPlayed;

    protected bool done;
    public bool ready;
    protected Camera viewCam;

    public virtual void Awake()
    {
        viewCam = Camera.main;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FPS_Controller>() && ready && !done)
        {
            DoEvent();
        }
    }

    public virtual void PlaySound()
    {
        StartCoroutine(ExecuteSoundEvent());
    }

    public IEnumerator ExecuteSoundEvent()
    {
        yield return new WaitForSeconds(soundEvent.delayAfterEvent);
        if (!(soundEvent.playOnce && soundPlayed))
        {
            soundPlayed = true;

            if (soundLocalisation != null)
                SoundManager.instance.PlayAudio(soundEvent.clip.name, soundLocalisation);
            else
                SoundManager.instance.PlayAudio(soundEvent.clip.name, transform);
        }
    }

    public virtual void DoEvent()
    {
        if (playSound && soundEvent)
        {
            if (soundEvent.onHold)
                PlaySound();
        }
    }
}
