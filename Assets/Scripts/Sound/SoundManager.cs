using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class Sound
{
    public string soundType => clip.name;
    public AudioClip clip;
    [Range(0, 1)] public float volume = 0.5f;
    [Range(-1, 1)] public float pitch = 1;
    public bool spatialBlend = true;
    public float radius = 20;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] AudioClip globalAmbient;

    [Header("Random sounds")]
    [SerializeField] Vector2 randomInterval;
    [SerializeField] AudioClip[] randomSounds;
    float randomDelay;
    float delay;

    [Header("Walking sounds")]
    [SerializeField] AudioClip[] walkSteps;

    [Header("Other sounds")]
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

    private void Start()
    {
        randomDelay = UnityEngine.Random.Range(randomInterval.x, randomInterval.y);
        lastSource = GenerateSound(globalAmbient);
        lastSource.loop = true;
        lastSource.Play();
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
            lastSource.spatialBlend = thisSound.spatialBlend ? 1 : 0;
            lastSource.minDistance = thisSound.radius;
            lastSource.dopplerLevel = 0;
            lastSource.pitch = thisSound.pitch;
            lastSource.Play();
        }
        else
            print("There is no song at this name : " + name);
    }

    public void RandomStep(Transform target)
    {
        int random = UnityEngine.Random.Range(0, walkSteps.Length);
        AudioClip step = walkSteps[random];
        lastSource = GenerateSound(step);
        lastSource.pitch = UnityEngine.Random.Range(0.75f, 1);
        lastSource.Play();
    }

    AudioSource GenerateSound(AudioClip clip)
    {
        GameObject sound = new GameObject();
        sound.transform.parent = transform;
        sound.name = clip.name;
        if (lastSource != null && lastSource.clip == clip && lastSource.isPlaying)
            Destroy(lastSource.gameObject);

        AudioSource source = sound.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0.1f;
        source.spatialBlend = 0;
        source.dopplerLevel = 0;
        source.pitch = 1;
        return source;
    }

    void PlayRandomSound()
    {
        randomDelay = UnityEngine.Random.Range(randomInterval.x, randomInterval.y);
        print(randomDelay);
        int random = UnityEngine.Random.Range(0, randomSounds.Length);
        GenerateSound(randomSounds[random]);
        lastSource.Play();
        delay = 0;
    }

    private void Update()
    {
        delay += Time.deltaTime;
        if (delay > randomDelay)
        {
            PlayRandomSound();
        }
    }
}
