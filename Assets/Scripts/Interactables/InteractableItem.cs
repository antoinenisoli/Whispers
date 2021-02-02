using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public abstract class InteractableItem : Interactable
{
    [Header("Item")]
    public float offset = 0.8f;
    protected Vector3 basePos;
    protected Quaternion baseRotation;
    protected Vector3 baseScale;
    protected bool isInspected;
    protected float animDuration = 0.5f;

    public override void Awake()
    {
        base.Awake();
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
        isInspected = true;
        PlayDialog();
        Vector3 centerOfCamera = viewCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0) + Vector3.forward * offset);
        transform.DOMove(centerOfCamera, animDuration);

        Quaternion lookAt = Quaternion.LookRotation(player.position - transform.position);
        Vector3 eulerAngles = lookAt.eulerAngles;
        lookAt.eulerAngles = eulerAngles;
        transform.DORotateQuaternion(lookAt, animDuration);
        
        if (playSound && soundEvent)
        {
            if (soundEvent.onHold)
                LaunchSoundEvent();
        }
    }

    public virtual void UnInspect()
    {
        isInspected = false;
        transform.DOMove(basePos, animDuration);
        transform.DORotateQuaternion(baseRotation, animDuration);
        transform.localScale = baseScale;
        if (doorToUnlock && doorToUnlock.locked)
            doorToUnlock.Unlock();

        if (doorToLock && !doorToLock.locked)
            doorToLock.Lock();

        if (playSound && soundEvent)
        {
            if (soundEvent.onPut)
                LaunchSoundEvent();
        }
    }

    private void Update()
    {
        if (!isInspected)
            meshRenderer.material = glowMat;
    }
}
