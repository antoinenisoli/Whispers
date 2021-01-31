using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticCard : SolidItem
{
    [SerializeField] Animator doorAnimator;
    bool animPlayed;

    public override void UnInspect()
    {
        base.UnInspect();
        if (!animPlayed)
        {
            doorAnimator.SetTrigger("openDoor");
            animPlayed = true;
        }
    }
}
