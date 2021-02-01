using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    [SerializeField] bool playVideo;
    [SerializeField] RawImage image;
    AudioSource music;
    VideoPlayer ps1video;

    private void Awake()
    {
        ps1video = FindObjectOfType<VideoPlayer>();
        music = GetComponentInChildren<AudioSource>();
    }

    private IEnumerator Start()
    {
        if (playVideo)
        {
            ps1video.Play();
            yield return new WaitForSeconds((float)ps1video.length);          
        }
        else
            yield return null;

        image.DOFade(0, 0.3f);
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
