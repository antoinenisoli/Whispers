using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableVolume : Interactable
{
    public override void Inspect(Transform player)
    {
        base.Inspect(player);
        thisCollider.isTrigger = true;
    }

    public override void UnInspect()
    {
        base.UnInspect();
        thisCollider.isTrigger = false;
    }
}
