using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        EventManager.instance.onGameStart.AddListener(StartBlink);
    }

    public void StartBlink()
    {
        anim.SetTrigger("start");
    }
}
