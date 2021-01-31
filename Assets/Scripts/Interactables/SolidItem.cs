using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidItem : InteractableItem
{
    public override void Inspect(Transform player)
    {
        base.Inspect(player);
        SoundManager.instance.PlayAudio("HoldItem", transform);
    }

    public override void UnInspect()
    {
        SoundManager.instance.PlayAudio("PutItem", transform);
        base.UnInspect();
    }
}
