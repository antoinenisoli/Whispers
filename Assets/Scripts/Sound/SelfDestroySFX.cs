using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroySFX : MonoBehaviour
{
    AudioSource source => GetComponent<AudioSource>();

    private void Awake()
    {
        source.Play();
        Destroy(gameObject, source.clip.length);
    }
}
