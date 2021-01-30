using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent : MonoBehaviour
{
    [SerializeField] Transform eventArea;
    [SerializeField] GameObject creepyThing;
    bool done;
    bool ready;

    [Header("Play Sound")]
    [SerializeField] bool playSound;
    [SerializeField] AudioClip clip;
    [SerializeField] Transform soundLocalisation;

    private void Awake()
    {
        creepyThing.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        FPS_Controller character = other.GetComponent<FPS_Controller>();
        if (character && !ready)
        {
            ready = true;
        }
    }

    IEnumerator ExecuteEvent()
    {
        if (playSound)
            SoundManager.instance.PlayAudio(clip.name, soundLocalisation);

        creepyThing.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        creepyThing.SetActive(false);
    }

    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToViewportPoint(eventArea.position);
        bool visible = screenPos.z > 0
            && screenPos.x > 0 && screenPos.x < 1
            && screenPos.y > 0 && screenPos.y < 1
            ;

        if (ready && visible && !done)
        {
            done = true;
            StartCoroutine(ExecuteEvent());
        }
    }
}
