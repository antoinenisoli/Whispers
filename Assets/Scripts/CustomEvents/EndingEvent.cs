using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingEvent : CustomEvent
{
    [Header("ENDING EVENT")]
    [SerializeField] Door doorToLock;
    [SerializeField] float fogValue = 20;

    public override void DoEvent()
    {
        base.DoEvent();
        if (ready)
            StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        done = true;
        if (doorToLock)
            doorToLock.FinalLock();

        yield return new WaitForSeconds(3);
        DOTween.To(() => RenderSettings.fogEndDistance, x => RenderSettings.fogEndDistance = x, fogValue, 3);
        SoundManager.instance.PlayAudio("GazSound", transform);

        yield return new WaitForSeconds(2);
        EventManager.instance.onEndGame.Invoke();
    }
}
