using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Computer : InteractableItem
{
    [Header("Computer")]
    [SerializeField] string goodText = "661";
    [SerializeField] InputField field;
    [SerializeField] Transform view;
    bool active;
    Vector3 camStartPos;
    Quaternion camStartRot;

    private void Start()
    {
        field.text = "_";
        field.gameObject.SetActive(false);
    }

    public override void Inspect(Transform player)
    {
        field.text = "_";
        viewCam.transform.DOMove(view.position, 0.3f);
        viewCam.transform.DORotateQuaternion(view.rotation, 0.3f);
        camStartPos = viewCam.transform.position;
        camStartRot = viewCam.transform.rotation;
        active = true;
        field.gameObject.SetActive(true);
    }

    public override void UnInspect()
    {
        field.text = "_";
        viewCam.transform.DOMove(camStartPos, 0.1f);
        viewCam.transform.DORotateQuaternion(camStartRot, 0.1f);
        active = false;
        field.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (active)
        {
            field.ActivateInputField();
            field.Select();

            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                if (field.text == goodText)
                {
                    print("yes");
                }

                field.text = "_";
            }
        }
    }
}
