using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OnSightEvent : CustomEvent
{
    [SerializeField] Transform eventArea;
    [SerializeField] bool translation;
    [SerializeField] float translationDuration = 1;

    [SerializeField] protected GameObject creepyThing;
    [SerializeField] float seeDuration = 1;
    [SerializeField] bool disappear;
    FPS_Controller player;

    private void Start()
    {
        player = FindObjectOfType<FPS_Controller>();
        if (creepyThing)
            creepyThing.SetActive(false);
    }

    public IEnumerator ShowThing()
    {
        creepyThing.SetActive(true);
        if (playSound && soundEvent.onPut)
            PlaySound();

        if (translation)
        {
            creepyThing.transform.DOLocalMove(creepyThing.transform.localPosition - Vector3.forward * 8, translationDuration);
        }

        yield return new WaitForSeconds(seeDuration);
        if (disappear)
            creepyThing.SetActive(false);
    }

    public void TriggerCondition()
    {
        if (ready && !done)
        {
            Vector3 screenPos = viewCam.WorldToViewportPoint(eventArea.position);
            bool visible = screenPos.z > 0
                && screenPos.x > 0 && screenPos.x < 1
                && screenPos.y > 0 && screenPos.y < 1
                && !Physics.Linecast(player.transform.position, creepyThing.transform.position);
                ;

            if (visible)
            {
                done = true;
                StartCoroutine(ShowThing());
            }
        }
    }

    private void Update()
    {
        TriggerCondition();
    }
}
