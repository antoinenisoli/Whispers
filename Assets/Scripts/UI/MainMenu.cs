﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    [SerializeField] RawImage image;
    VideoPlayer ps1video;

    private IEnumerator Start()
    {
        ps1video = FindObjectOfType<VideoPlayer>();
        yield return new WaitForSeconds((float)ps1video.length);
        image.DOFade(0, 0.3f);
        image.raycastTarget = false;
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