using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLightEvent : CustomEvent
{
    [Header("LIGHT EVENT")]
    [SerializeField] Light _light;
    [SerializeField] bool automatic = true;
    [SerializeField] Vector2 randomTimer = new Vector2(0.2f, 0.4f);
    float timer;
    float delay;

    public override void Awake()
    {
        base.Awake();
        timer = Random.Range(randomTimer.x, randomTimer.y);
    }

    IEnumerator Flicker()
    {
        delay = 0;
        while (delay < timer)
        {
            delay += Time.deltaTime;
            yield return null;
        }

        timer = Random.Range(randomTimer.x, randomTimer.y);
        delay = 0;
        _light.enabled = !_light.enabled;
    }

    public override void DoEvent()
    {
        base.DoEvent();
        if (!automatic && ready)
            StartCoroutine(Flicker());
    }

    private void Update()
    {
        if (automatic && ready)
        {
            delay += Time.deltaTime;
            if (delay > timer)
            {
                timer = Random.Range(randomTimer.x, randomTimer.y);
                delay = 0;
                _light.enabled = !_light.enabled;
            }
        }
    }
}
