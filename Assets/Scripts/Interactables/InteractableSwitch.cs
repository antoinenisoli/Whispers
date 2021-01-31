using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableSwitch : Interactable
{
    [Header("SWITCH")]
    public bool locked;

    public virtual void Effect()
    {
        if (playSound && soundEvent)
        {
            if (soundEvent.onHold)
                LaunchSoundEvent();
        }
    }
}
