using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("Cursor")]
    public Image cursorImage;
    [SerializeField] Sprite normalCursor, lockedCursor;

    [Header("Blink")]
    [SerializeField] Animator blinkAnim;

    private void Start()
    {
        EventManager.instance.onGameStart.AddListener(StartBlink);
    }

    public void StartBlink()
    {
        blinkAnim.SetTrigger("start");
    }

    public void LockCursor(bool isLocked)
    {
        cursorImage.sprite = isLocked ? lockedCursor : normalCursor;
    }
}
