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
    [SerializeField] protected Material glowMat;
    protected Material baseMat;
    protected MeshRenderer meshRenderer;
    protected Collider thisCollider;
    [SerializeField] SoundEvent soundEvent;
    public bool done;
    [SerializeField] protected bool playDialog;
    [SerializeField] protected DialogInfo myDialog;
    [SerializeField] protected Door doorToOpen;

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

    public void LaunchSoundEvent()
    {
        if (!done)
            StartCoroutine(ExecuteEvent());
    }

    public void PlayDialog()
    {
        if (playDialog)
            EventManager.instance.OnDialog.Invoke(myDialog);
    }

    IEnumerator ExecuteEvent()
    {
        yield return new WaitForSeconds(soundEvent.delayAfterEvent);
        if (soundEvent.playSound)
        {
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
