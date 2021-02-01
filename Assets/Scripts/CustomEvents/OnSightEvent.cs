using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OnSightEvent : OnEnterEvent
{
    [Header("ON SIGHT")]
    [SerializeField] Transform eventArea;
    [SerializeField] float seeDuration = 1;
    [SerializeField] bool disappear;

    [Header("Animation")]
    [SerializeField] bool translation;
    [SerializeField] Vector3 direction;
    [SerializeField] float translationDuration = 1;

    public override void Awake()
    {
        base.Awake();
        ready = false;
    }

    public override IEnumerator ShowThing()
    {
        yield return base.ShowThing();
        if (doorToAffect)
            doorToAffect.Effect();

        if (playSound && soundEvent)
        {
            if (soundEvent.onPut)
                PlaySound();
        }

        if (translation)
        {
            creepyThing.transform.DOLocalMove(creepyThing.transform.localPosition + direction * 8, translationDuration);
        }

        yield return new WaitForSeconds(seeDuration);
        if (disappear)
            creepyThing.SetActive(false);
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FPS_Controller>() && !done)
        {
            if (playSound && soundEvent)
            {
                if (soundEvent.onHold)
                    PlaySound();
            }

            ready = true;
        }
    }

    public override void DoEvent()
    {
        StartCoroutine(ShowThing());
    }

    public void TriggerCondition()
    {
        if (ready && !done)
        {
            Vector3 screenPos = viewCam.WorldToViewportPoint(eventArea.position);
            bool visible = screenPos.z > 0
                && screenPos.x > 0 && screenPos.x < 1
                && screenPos.y > 0 && screenPos.y < 1
                && !Physics.Linecast(player.transform.position, eventArea.transform.position);
                ;

            if (visible)
            {
                DoEvent();
            }
        }
    }

    private void Update()
    {
        TriggerCondition();
    }
}
