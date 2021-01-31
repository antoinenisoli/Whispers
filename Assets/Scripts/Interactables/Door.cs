using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : InteractableSwitch
{
    [Header("Door")]
    [SerializeField] float animDuration = 1;
    [SerializeField] float doorAngle = 120;
    Vector3 startRot;
    bool open;
    Vector3 rotation;
    bool gameEnded;

    public override void Awake()
    {
        base.Awake();
        startRot = transform.localRotation.eulerAngles;
    }

    public override void Start()
    {
        base.Start();
        EventManager.instance.onEndGame.AddListener(FinalLock);
    }

    void FinalLock()
    {
        gameEnded = true;
        thisCollider.isTrigger = false;
        transform.DOLocalRotate(startRot, 0.3f);
    }

    public void Unlock()
    {
        locked = false;
        open = true;
        SoundManager.instance.PlayAudio("UnlockDoor", transform);
    }

    public void Lock()
    {
        locked = true;
        transform.DOKill();
        thisCollider.isTrigger = false;
        open = false;
        if (open)
            SoundManager.instance.PlayAudio("DoorSqueak", transform);

        StartCoroutine(Reset());
        SoundManager.instance.PlayAudio("LockDoor", transform);
    }

    void Switch()
    {
        transform.DOKill();
        open = !open;
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
        thisCollider.isTrigger = open;
        busy = false;
    }
}
