using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameOverAnimation : MonoBehaviour
{
    [SerializeField] Image fadeScreen;
    [SerializeField] Text endText;
    [SerializeField] float fadeDuration = 5;

    private void Start()
    {
        EventManager.instance.onEndGame.AddListener(EndGame);
        fadeScreen.DOFade(0, 0);
        endText.DOFade(0, 0);
    }

    IEnumerator EndAnim()
    {
        yield return new WaitForSeconds(5f);
        fadeScreen.DOFade(1, fadeDuration);
        endText.DOFade(1, fadeDuration).SetDelay(1);

        yield return new WaitForSeconds(fadeDuration + 3);
        SceneManager.LoadScene(0);
    }

    void EndGame()
    {
        StartCoroutine(EndAnim());
    }
}
