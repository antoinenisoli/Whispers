using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public UnityEvent onDoorUnlocked = new UnityEvent();
    public UnityEvent onGameStart = new UnityEvent();
    public UnityEvent onEndGame = new UnityEvent();
    public class AudioLogEvent : UnityEvent<AudioProfile> { }
    public AudioLogEvent onPlayLog = new AudioLogEvent();

    public class DialogEvent : UnityEvent<DialogInfo> { }
    public DialogEvent onDialog = new DialogEvent();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
