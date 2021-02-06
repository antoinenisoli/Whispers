using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Image fading;
    [SerializeField] AudioSource jingle;
    [SerializeField] AudioSource music;

    private void Awake()
    {
        image.DOFade(0, 0f);
        fading.DOFade(1, 0f);
    }

    private IEnumerator Start()
    {
        image.raycastTarget = true;
        image.DOFade(1, 1f);
        yield return new WaitForSeconds(jingle.clip.length - 0.25f);
        image.DOFade(0, 1f);
        fading.DOFade(0, 1f);
        yield return new WaitForSeconds(1);
        image.raycastTarget = false;
        music.Play();
    }

    public void LaunchLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
