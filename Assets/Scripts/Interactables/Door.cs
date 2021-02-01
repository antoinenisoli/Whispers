using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : InteractableSwitch
{
    [Header("Door")]
    [SerializeField] MeshCollider doorCollider;
    [SerializeField] float animDuration = 1;
    [SerializeField] float doorAngle = 120;
    Vector3 startRot;
    bool open;
    Vector3 rotation;
    [HideInInspector] public bool finalRoom;

    public override void Awake()
    {
        base.Awake();
        startRot = transform.localRotation.eulerAngles;
    }

    public void Unlock()
    {
        locked = false;
        open = true;
        doorCollider.convex = false;
        EventManager.instance.onDoorUnlocked.Invoke();
        transform.DOLocalRotate(new Vector3(-90, 0, doorAngle), animDuration);
        SoundManager.instance.PlayAudio("UnlockDoor", transform);
    }

    public void Lock()
    {
        locked = true;
        transform.DOKill();
        doorCollider.convex = true;
        open = false;
        transform.DOLocalRotate(startRot, animDuration);
        if (open)
            SoundManager.instance.PlayAudio("DoorSqueak", transform);

        StartCoroutine(Reset());
        SoundManager.instance.PlayAudio("LockDoor", transform);
    }

    public void FinalLock()
    {
        locked = true;
        finalRoom = false;
        transform.DOKill();
        doorCollider.convex = true;
        open = false;
        transform.DOLocalRotate(startRot, 0.2f);
        SoundManager.instance.PlayAudio("LockDoor", transform);
    }

    void Switch()
    {
        transform.DOKill();
        open = !open;
        doorCollider.convex = open ? false : true;
        SoundManager.instance.PlayAudio("DoorSqueak", transform);
        StartCoroutine(Reset());
    }

    public override void Effect()
    {
        if (!busy && !locked)
        {
            Switch();
            rotation = open ? new Vector3(-90, 0, doorAngle) : startRot;
            transform.DOLocalRotate(rotation, animDuration);
            if (playSound && soundEvent)
            {
                if (open && soundEvent.onHold)
                    LaunchSoundEvent();
                else if (soundEvent.onPut)
                    LaunchSoundEvent();
            }
        }
    }

    IEnumerator Reset()
    {
        busy = true;
        yield return new WaitForSeconds(animDuration);
        busy = false;
    }

    private void Update()
    {
        if (finalRoom)
            meshRenderer.material = glowMat;
    }
}
