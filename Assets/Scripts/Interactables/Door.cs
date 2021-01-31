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
    [SerializeField] bool locked;
    Vector3 rotation;

    public override void Awake()
    {
        base.Awake();
        startRot = transform.localRotation.eulerAngles;
    }

    public void Unlock()
    {
        locked = false;
        SoundManager.instance.PlayAudio("UnlockDoor", transform);
    }

    public override void Effect()
    {
        base.Effect();
        if (!busy && !locked)
        {
            transform.DOKill();
            open = !open;
            StartCoroutine(Reset());
        }
    }

    IEnumerator Reset()
    {
        busy = true;
        yield return new WaitForSeconds(animDuration);
        thisCollider.isTrigger = open;
        busy = false;
    }

    private void Update()
    {
        rotation = open ? new Vector3(-90, 0, doorAngle) : startRot;
        transform.DOLocalRotate(rotation, animDuration);
    }
}
