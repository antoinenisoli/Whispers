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
    [SerializeField] Image finalImage;
    [SerializeField] Sprite[] frames;

    private void Start()
    {
        EventManager.instance.onEndGame.AddListener(EndGame);
        fadeScreen.DOFade(0, 0);
        finalImage.DOFade(0, 0);
        finalImage.sprite = frames[0];
        endText.DOFade(0, 0);
    }

    IEnumerator EndAnim()
    {
        yield return new WaitForSeconds(5f);
        fadeScreen.DOFade(1, fadeDuration);

        yield return new WaitForSeconds(fadeDuration/2);
        finalImage.DOFade(1, 3);

        yield return new WaitForSeconds(5);
        finalImage.DOFade(0, 2);

        yield return new WaitForSeconds(2f);
        finalImage.sprite = frames[1];
        finalImage.DOFade(1, 2);

        yield return new WaitForSeconds(5);
        endText.DOFade(1, 2.5f);

        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }

    void EndGame()
    {
        StartCoroutine(EndAnim());
    }
}
