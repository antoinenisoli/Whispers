using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableSwitch : Interactable
{
    [Header("SWITCH")]
    public bool busy;

    public virtual void Effect()
    {
        LaunchSoundEvent();
    }
}
