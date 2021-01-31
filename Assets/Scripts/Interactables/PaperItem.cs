using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperItem : InteractableItem
{
    public override void Inspect(Transform player)
    {
        base.Inspect(player);
        SoundManager.instance.PlayAudio("HoldPaper", transform);
    }

    public override void UnInspect()
    {
        SoundManager.instance.PlayAudio("PutPaper", transform);
        base.UnInspect();
    }
}
