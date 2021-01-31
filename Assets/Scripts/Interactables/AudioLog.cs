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
        yield return new WaitForSeconds(0.7f);
        EventManager.instance.OnPlayLog.Invoke(audiologProfile);
        EventManager.instance.OnDialog.Invoke(audiologProfile);
        SoundManager.instance.PlayAudio(audiologProfile.clip.name, transform);
        yield return new WaitForSeconds(audiologProfile.clip.length);
        busy = false;
        yield return new WaitForSeconds(1);
        PlayDialog();

        if (playSound && soundEvent)
        {
            if (soundEvent.onPut)
                LaunchSoundEvent();
        }
    }

    public override void Effect()
    {
        base.Effect();
        if (busy)
            return;

        busy = true;
        StartCoroutine(End());
        SoundManager.instance.PlayAudio("AudiologRewind", transform);
    }
}
