using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterEvent : CustomEvent
{
    [Header("ON ENTER")]
    [SerializeField] protected float wait = 0;
    [SerializeField] protected GameObject creepyThing;
    [SerializeField] protected Door doorToAffect;
    protected FPS_Controller player;

    private void Start()
    {
        player = FindObjectOfType<FPS_Controller>();
        if (creepyThing)
            creepyThing.SetActive(false);
    }

    public virtual IEnumerator ShowThing()
    {
        done = true;
        if (creepyThing)
            creepyThing.SetActive(true);
        yield return null;
    }

    public override void DoEvent()
    {
        base.DoEvent();
        if (creepyThing)
            StartCoroutine(ShowThing());
    }
}
