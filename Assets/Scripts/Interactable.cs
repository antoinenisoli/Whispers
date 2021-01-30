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

public class Interactable : MonoBehaviour
{
    [SerializeField] float offset = 0.8f;
    [SerializeField] InteractionType interaction;
    Vector3 basePos;
    Quaternion baseRotation;
    Vector3 baseScale;
    Collider thisCollider;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        thisCollider = GetComponent<Collider>();
        basePos = transform.position;
        baseRotation = transform.rotation;
        baseScale = transform.localScale;
    }

    public void Rotate(float rotSpeed)
    {
        Vector3 mouse;
        switch (interaction)
        {
            case InteractionType.Polaroid:
                mouse = Vector3.forward * Input.GetAxis("Mouse X");
                transform.Rotate(mouse * Time.deltaTime * rotSpeed);
                break;
            case InteractionType.VolumeObject:
                mouse = new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"));
                transform.Rotate(mouse * Time.deltaTime * rotSpeed);
                break;
        }
    }

    public void Inspect(Transform player)
    {
        thisCollider.isTrigger = true;
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

    public void UnInspect()
    {
        thisCollider.isTrigger = false;
        rb.isKinematic = false;
        transform.DOMove(basePos, 0.5f);
        transform.DORotateQuaternion(baseRotation, 0.5f);
        transform.localScale = baseScale;
    }
}
