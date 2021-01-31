using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundEvent", menuName = "Event/Sounds")]
public class SoundEvent : ScriptableObject
{
    public bool playOnce;
    public bool onHold;
    public bool onPut;
    public float delayAfterEvent;
    public AudioClip clip;
}
