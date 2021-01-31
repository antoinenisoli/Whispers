using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSightEvent : CustomEvent
{
    [SerializeField] Transform eventArea;

    public IEnumerator ExecuteEvent()
    {
        creepyThing.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        creepyThing.SetActive(false);
        yield return new WaitForSeconds(soundEvent.delayAfterEvent);
        if (soundEvent.playSound)
        {
            if (soundEvent.soundLocalisation != null)
                SoundManager.instance.PlayAudio(soundEvent.clip.name, soundEvent.soundLocalisation);
            else
                SoundManager.instance.PlayAudio(soundEvent.clip.name, transform);
        }
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
                StartCoroutine(ExecuteEvent());
            }
        }
    }

    private void Update()
    {
        TriggerCondition();
    }
}
