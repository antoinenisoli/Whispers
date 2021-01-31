using System.Collections;
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
    }

    public override void UnInspect()
    {
        field.text = "_";
        viewCam.transform.DOMove(camStartPos, 0.1f);
        viewCam.transform.DORotateQuaternion(camStartRot, 0.1f);
        inInspection = false;
    }

    void ManageComputer()
    {
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
                    if (doorToUnlock)
                        doorToUnlock.Unlock();

                    if (doorToLock)
                        doorToLock.Lock();
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
