﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Computer : InteractableItem
{
    [Header("Computer")]
    [SerializeField] string goodText = "661";
    [SerializeField] GameObject validedScreen;
    [SerializeField] InputField field;
    [SerializeField] Transform view;
    [SerializeField] CustomEvent eventTotrigger;
    bool inInspection;
    Vector3 camStartPos;
    Quaternion camStartRot;

    public override void Awake()
    {
        base.Awake();
        field.text = "_";
        validedScreen.SetActive(false);
    }

    public override void Inspect(Transform player)
    {
        field.text = "_";
        viewCam.transform.DOMove(view.position, 0.3f);
        viewCam.transform.DORotateQuaternion(view.rotation, 0.3f);
        camStartPos = viewCam.transform.position;
        camStartRot = viewCam.transform.rotation;
        inInspection = true;

        if (playSound && soundEvent)
        {
            if (soundEvent.onHold)
                LaunchSoundEvent();
        }
    }

    public override void Rotate(float rotSpeed)
    {
        
    }

    public override void UnInspect()
    {
        field.text = "_";
        viewCam.transform.DOMove(camStartPos, 0.1f);
        viewCam.transform.DORotateQuaternion(camStartRot, 0.1f);
        inInspection = false;

        if (playSound && soundEvent)
        {
            if (soundEvent.onPut)
                LaunchSoundEvent();
        }
    }

    IEnumerator Door()
    {
        yield return new WaitForSeconds(1);
        if (doorToUnlock)
            doorToUnlock.Unlock();

        if (doorToLock)
        {
            doorToLock.Lock();
            SoundManager.instance.PlayAudio("LoudDoorClap", transform);
        }
    }

    void ManageComputer()
    {
        if (!done)
            meshRenderer.material = glowMat;

        if (inInspection && !done)
        {
            field.ActivateInputField();
            field.Select();

            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                if (field.text.Equals(goodText))
                {
                    done = true;
                    PlayDialog();
                    validedScreen.SetActive(true);
                    if (eventTotrigger)
                        eventTotrigger.ready = true;

                    StartCoroutine(Door());
                }
                else
                {
                    SoundManager.instance.PlayAudio("WrongCode", transform);
                }

                field.text = "_";
            }
        }
    }

    private void Update()
    {
        ManageComputer();
    }
}
