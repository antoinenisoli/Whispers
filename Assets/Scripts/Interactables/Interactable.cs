using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum InteractionType
{
    Polaroid,
    VolumeObject,
}

public abstract class Interactable : MonoBehaviour
{
    [Header("Interactable")]
    protected Camera viewCam;
    [SerializeField] protected Material glowMat;
    protected Material baseMat;
    protected MeshRenderer meshRenderer;
    protected Collider thisCollider;

    [Header("Play Dialog")]
    [SerializeField] protected bool playDialog;
    [SerializeField] protected AudioClip clip;

    public virtual void Awake()
    {
        viewCam = Camera.main;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        thisCollider = GetComponent<Collider>();
        baseMat = meshRenderer.material;
    }

    public void HighLight(bool b)
    {
        meshRenderer.material = b ? glowMat : baseMat;
    }
}
