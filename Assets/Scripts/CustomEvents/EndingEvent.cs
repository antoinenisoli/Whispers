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
    [SerializeField] float endingDuration = 5;

    private void Start()
    {
        fadeScreen.DOFade(0,0);
        endText.DOFade(0,0);
    }

    public override void DoEvent()
    {
        base.DoEvent();
        StartCoroutine(EndGame());
        EventManager.instance.onEndGame.Invoke();
        fadeScreen.DOFade(1, endingDuration);
        endText.DOFade(1, endingDuration).SetDelay(1);
        DOTween.To(() => RenderSettings.fogStartDistance, x => RenderSettings.fogStartDistance = x, -200, endingDuration);
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(endingDuration + 3);
        SceneManager.LoadScene(0);
    }
}
