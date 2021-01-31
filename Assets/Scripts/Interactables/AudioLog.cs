using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AudioLog : InteractableSwitch
{
    [Header("Audio Log")]
    [SerializeField] AudioProfile audiologProfile;

    IEnumerator End()
    {
        yield return new WaitForSeconds(audiologProfile.clip.length);
        busy = false;
        yield return new WaitForSeconds(1);
        PlayDialog();
    }

    public override void Effect()
    {
        base.Effect();
        if (busy)
            return;

        done = true;
        busy = true;
        StartCoroutine(End());
        EventManager.instance.OnPlayLog.Invoke(audiologProfile);
        EventManager.instance.OnDialog.Invoke(audiologProfile);
        SoundManager.instance.PlayAudio(audiologProfile.clip.name, transform);
    }
}
