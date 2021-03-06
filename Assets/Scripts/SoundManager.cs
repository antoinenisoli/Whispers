﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Serializable]
    class Sound
    {
        public string soundType;
        public AudioClip clip;
        [Range(0, 1)]
        public float volume = 0.5f;
        [Range(-1, 1)]
        public float pitch = 1;
        public bool spatialBlend = true;
        public float radius = 20;
    }

    public static SoundManager instance;
    [SerializeField] Sound[] sounds = new Sound[1];
    Dictionary<string, Sound> soundsLibrary = new Dictionary<string, Sound>();
    AudioSource lastSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        foreach (var s in sounds)
            if (!soundsLibrary.ContainsKey(s.soundType))
                soundsLibrary.Add(s.soundType, s);
    }

    public void PlayAudio(string name, Transform target)
    {
        if (soundsLibrary.TryGetValue(name, out Sound thisSound))
        {
            GameObject sound = new GameObject();
            sound.transform.position = target.position;
            sound.name = thisSound.clip.name;
            sound.transform.parent = transform;
            if (lastSource != null && lastSource.clip == thisSound.clip && lastSource.isPlaying)
                Destroy(lastSource.gameObject);

            lastSource = sound.AddComponent<AudioSource>();
            lastSource.clip = thisSound.clip;
            lastSource.volume = thisSound.volume;
            lastSource.pitch = thisSound.pitch;
            lastSource.spatialBlend = thisSound.spatialBlend ? 1 : 0;
            lastSource.minDistance = thisSound.radius;
            lastSource.dopplerLevel = 0;
            lastSource.Play();
        }
        else
            print("There is no song at this name : " + name);
    }
}
