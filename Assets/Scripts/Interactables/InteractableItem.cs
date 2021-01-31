using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public abstract class InteractableItem : Interactable
{
    [Header("Item")]
    public float offset = 0.8f;
    Vector3 basePos;
    Quaternion baseRotation;
    Vector3 baseScale;
    Rigidbody rb;
    Vector3 posLastFrame;

    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        basePos = transform.position;
        baseRotation = transform.rotation;
        baseScale = transform.localScale;
    }

    public virtual void Rotate(float rotSpeed)
    {
        Vector3 mouse;
        if (Input.GetMouseButton(0))
        {
            mouse = new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0);
            transform.Rotate(mouse * Time.deltaTime * rotSpeed);
        }

        /*if (Input.GetMouseButtonDown(0))
            posLastFrame = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            var delta = Input.mousePosition - posLastFrame;
            posLastFrame = Input.mousePosition;
            var axis = Quaternion.AngleAxis(0, Vector3.forward) * delta;
            transform.rotation = Quaternion.AngleAxis(delta.magnitude * 0.8f, axis) * transform.rotation;
        }*/
    }

    public virtual void Inspect(Transform player)
    {
        PlayDialog();

        rb.isKinematic = true;
        Vector3 centerOfCamera = viewCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0) + Vector3.forward * offset);
        transform.DOMove(centerOfCamera, 0.5f);

        Quaternion lookAt = Quaternion.LookRotation(player.position - transform.position);
        Vector3 eulerAngles = lookAt.eulerAngles;
        lookAt.eulerAngles = eulerAngles;
        transform.DORotateQuaternion(lookAt, 0.5f);
    }

    public virtual void UnInspect()
    {
        rb.isKinematic = false;
        transform.DOMove(basePos, 0.5f);
        transform.DORotateQuaternion(baseRotation, 0.5f);
        transform.localScale = baseScale;
        LaunchSoundEvent();
        if (doorToUnlock && doorToUnlock.locked)
            doorToUnlock.Unlock();

        if (doorToLock && !doorToLock.locked)
            doorToLock.Lock();
    }
}
