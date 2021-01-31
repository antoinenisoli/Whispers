using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSightEvent : CustomEvent
{
    [SerializeField] Transform eventArea;
    [SerializeField] protected GameObject creepyThing;

    private void Start()
    {
        if (creepyThing)
            creepyThing.SetActive(false);
    }

    public IEnumerator ShowThing()
    {
        if (playSound && soundEvent.onPut)
            PlaySound();

        creepyThing.SetActive(true);
        yield return new WaitForSeconds(0.5f);
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
