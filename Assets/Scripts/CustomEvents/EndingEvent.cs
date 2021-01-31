using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingEvent : CustomEvent
{
    [SerializeField] Image fadeScreen;
    [SerializeField] Text endText;
    [SerializeField] float fadeDuration = 5;
    [SerializeField] Door doorToLock;

    private void Start()
    {
        fadeScreen.DOFade(0,0);
        endText.DOFade(0,0);
    }

    public override void DoEvent()
    {
        base.DoEvent();
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        if (doorToLock)
            doorToLock.Lock();

        yield return new WaitForSeconds(3);
        DOTween.To(() => RenderSettings.fogStartDistance, x => RenderSettings.fogStartDistance = x, -200, 3);
        SoundManager.instance.PlayAudio("GazSound", transform);

        yield return new WaitForSeconds(2);
        EventManager.instance.onEndGame.Invoke();

        yield return new WaitForSeconds(5f);
        fadeScreen.DOFade(1, fadeDuration);
        endText.DOFade(1, fadeDuration).SetDelay(1);

        yield return new WaitForSeconds(fadeDuration + 3);
        SceneManager.LoadScene(0);
    }
}
