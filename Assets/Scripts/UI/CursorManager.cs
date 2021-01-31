using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Sprite normal, locked;
    Image myImage;

    public void Lock(bool isLocked)
    {
        myImage.sprite = isLocked ? locked : normal;
    }

    private void Awake()
    {
        myImage = GetComponentInChildren<Image>();
    }
}
