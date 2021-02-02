using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MagneticCard : SolidItem
{
    [SerializeField] Animator doorAnimator;
    bool animPlayed;

    public override void UnInspect()
    {
        isInspected = false;
        if (doorToLock && !doorToLock.locked)
            doorToLock.Lock();

        if (playSound && soundEvent)
        {
            if (soundEvent.onPut)
            {
                if (soundLocalisation != null)
                    SoundManager.instance.PlayAudio(soundEvent.clip.name, soundLocalisation);
                else
                    SoundManager.instance.PlayAudio(soundEvent.clip.name, transform);
            }
        }

        if (!animPlayed)
        {
            doorAnimator.SetTrigger("openDoor");
            animPlayed = true;
        }

        if (doorToUnlock)
        {
            doorToUnlock.locked = false;
            doorToUnlock.finalRoom = true;
        }

        Destroy(gameObject);
    }
}
