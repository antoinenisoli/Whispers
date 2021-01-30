using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class AudioProfile
{
    public Sprite portrait;
    public AudioClip myDialog;
}

public class AudioLog : InteractableSwitch
{
    [Header("Audio Log")]
    [SerializeField] AudioProfile audiologProfile;

    IEnumerator End()
    {
        yield return new WaitForSeconds(audiologProfile.myDialog.length);
        busy = false;
    }

    public override void Effect()
    {
        if (busy)
            return;

        busy = true;
        StartCoroutine(End());
        EventManager.instance.OnPlayLog.Invoke(audiologProfile);
        SoundManager.instance.PlayAudio(audiologProfile.myDialog.name, transform);
    }
}
