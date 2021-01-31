using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSightEvent : CustomEvent
{
    [SerializeField] Transform eventArea;
    [SerializeField] protected GameObject creepyThing;
    [SerializeField] float seeDuration = 1;
    FPS_Controller player;

    private void Start()
    {
        player = FindObjectOfType<FPS_Controller>();
        if (creepyThing)
            creepyThing.SetActive(false);
    }

    public IEnumerator ShowThing()
    {
        if (playSound && soundEvent.onPut)
            PlaySound();

        creepyThing.SetActive(true);
        yield return new WaitForSeconds(seeDuration);
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
