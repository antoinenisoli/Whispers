using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class Interactable : MonoBehaviour
{
    [Header("Interactable")]
    protected Camera viewCam;
    public bool done;
    [SerializeField] protected Material glowMat;
    protected Material baseMat;
    protected MeshRenderer meshRenderer;
    protected Collider thisCollider;

    [Header("Sound Event")]
    [SerializeField] protected bool playOnce;
    protected bool played;
    [SerializeField] protected SoundEvent soundEvent;

    [Header("Dialog")]
    [SerializeField] protected bool playDialog;
    [SerializeField] protected DialogInfo myDialog;

    [Header("Doors")]
    [SerializeField] protected Door doorToUnlock;
    [SerializeField] protected Door doorToLock;

    public virtual void Awake()
    {
        viewCam = Camera.main;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        thisCollider = GetComponent<Collider>();
        baseMat = meshRenderer.material;
    }

    public virtual void Start()
    {
        EventManager.instance.OnDialog.AddListener(PlayDialog);
    }

    public virtual void LaunchSoundEvent()
    {
        if (!done && !(played && playOnce))
            StartCoroutine(ExecuteSoundEvent());
    }

    public void PlayDialog()
    {
        if (playDialog)
            EventManager.instance.OnDialog.Invoke(myDialog);
    }

    public IEnumerator ExecuteSoundEvent()
    {
        yield return new WaitForSeconds(soundEvent.delayAfterEvent);
        if (soundEvent.playSound)
        {
            played = true;
            if (soundEvent.soundLocalisation != null)
                SoundManager.instance.PlayAudio(soundEvent.clip.name, soundEvent.soundLocalisation);
            else
                SoundManager.instance.PlayAudio(soundEvent.clip.name, transform);
        }
    }

    void PlayDialog(DialogInfo info)
    {
        SoundManager.instance.PlayAudio(info.clip.name, transform);
    }

    public void HighLight(bool b)
    {
        meshRenderer.material = b ? glowMat : baseMat;
    }
}
