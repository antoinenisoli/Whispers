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
    public float offset = 0.8f;
    [SerializeField] Material glowMat;
    MeshRenderer meshRenderer;
    Material baseMat;
    Vector3 basePos;
    Quaternion baseRotation;
    Vector3 baseScale;
    protected Collider thisCollider;
    Rigidbody rb;
    Vector3 posLastFrame;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        thisCollider = GetComponent<Collider>();
        basePos = transform.position;
        baseRotation = transform.rotation;
        baseScale = transform.localScale;
        meshRenderer = GetComponent<MeshRenderer>();
        baseMat = meshRenderer.material;
    }

    public virtual void Rotate(float rotSpeed)
    {
        Vector3 mouse;
        if (Input.GetMouseButton(0))
        {
            mouse = new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0);
            transform.Rotate(mouse * Time.deltaTime * rotSpeed);
        }

        /*if (Input.GetMouseButtonDown(0))
            posLastFrame = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            var delta = Input.mousePosition - posLastFrame;
            posLastFrame = Input.mousePosition;
            var axis = Quaternion.AngleAxis(0, Vector3.forward) * delta;
            transform.rotation = Quaternion.AngleAxis(delta.magnitude * 0.8f, axis) * transform.rotation;
        }*/
    }

    public virtual void Inspect(Transform player)
    {
        rb.isKinematic = true;
        SoundManager.instance.PlayAudio("Test_3", transform);
        Vector3 centerOfCamera = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0) + Vector3.forward * offset);
        transform.DOMove(centerOfCamera, 0.5f);

        Quaternion lookAt = Quaternion.LookRotation(player.position - transform.position);
        Vector3 eulerAngles = lookAt.eulerAngles;
        eulerAngles.x = 70;
        lookAt.eulerAngles = eulerAngles;
        transform.DORotateQuaternion(lookAt, 0.5f);
    }

    public void HighLight(bool b)
    {
        meshRenderer.material = b ? glowMat : baseMat;
    }

    public virtual void UnInspect()
    {
        rb.isKinematic = false;
        transform.DOMove(basePos, 0.5f);
        transform.DORotateQuaternion(baseRotation, 0.5f);
        transform.localScale = baseScale;
    }
}
