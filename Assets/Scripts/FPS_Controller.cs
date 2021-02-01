using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

public class FPS_Controller : MonoBehaviour
{
    [SerializeField] Camera viewCam;
    CursorManager cursor;
    Drunk drunk;
    bool doorLocked;
    Rigidbody rb;
    bool isDead;
    PostProcessVolume volume;

    [Header("Movements")]
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float camSensitivity = 150;
    [SerializeField] float camBounds = 40;
    [SerializeField] [Range(0,1)] float walkCadency = 1f;
    float rotY;
    float walkingTime;

    [Header("Head bobbing")]
    [SerializeField] Transform head;
    [SerializeField] float bobFrequency;
    [SerializeField] float bobHorizontalAmplitude = 0.1f;
    [SerializeField] float bobVerticalAmplitude = 0.1f;
    [Range(0, 1)] [SerializeField] float headBobSmoothing = 0.1f;
    Vector3 targetCameraPosition;

    [Header("Interactions")]
    [SerializeField] LayerMask interactLayer;
    [SerializeField] Transform interactionPoint;
    [SerializeField] float interactionRange = 5;
    [SerializeField] float rotSpeed = 1;
    [SerializeField] Interactable usableItem;
    InteractableItem inspectedObject;
    bool inspectMode;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(viewCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)), viewCam.transform.forward * interactionRange);
    }

    private void Awake()
    {
        cursor = FindObjectOfType<CursorManager>();
        viewCam = Camera.main;
        rb = GetComponent<Rigidbody>();
        volume = FindObjectOfType<PostProcessVolume>();
        drunk = viewCam.GetComponent<Drunk>();
        drunk.enabled = false;
    }

    private void Start()
    {
        EventManager.instance.onEndGame.AddListener(StopPlayer);
        EventManager.instance.onGameStart.Invoke();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void StopPlayer()
    {
        drunk.enabled = true;
    }

    void FPS_Move()
    {
        Vector3 horizontalAxis = transform.right * Input.GetAxisRaw("Horizontal");
        Vector3 verticalAxis = transform.forward * Input.GetAxisRaw("Vertical");
        Vector3 normalized = (horizontalAxis + verticalAxis).normalized;

        Vector3 move = normalized * moveSpeed;
        move.y = 0;
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);

        if (move.magnitude > 0.1f)
        {
            walkingTime += Time.deltaTime;
            if (walkingTime > walkCadency)
            {
                walkingTime = 0;
                SoundManager.instance.RandomStep();
            }
        }
        else
            walkingTime = walkCadency/2;
    }

    void CameraControl()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector2 mouseInput = new Vector2(mouseX, mouseY) * camSensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - mouseInput.x, transform.rotation.eulerAngles.z);

        rotY += mouseY * camSensitivity * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, -camBounds, camBounds);
        viewCam.transform.rotation = Quaternion.Euler(rotY, viewCam.transform.rotation.eulerAngles.y, viewCam.transform.rotation.eulerAngles.z);
    }

    void ChangeDepth(float value)
    {
        if (volume != null)
        {
            if (volume.profile.TryGetSettings(out DepthOfField depthOfField))
            {
                DOTween.To(() => depthOfField.focusDistance.value, x => depthOfField.focusDistance.value = x, value, 0.3f);
            }
        }
    }

    void Interact()
    {
        if (inspectMode)
        {
            inspectedObject.HighLight(false);
            inspectedObject.Rotate(rotSpeed);

            if (Input.GetMouseButton(1))
            {
                inspectMode = false;
                inspectedObject.UnInspect();
                inspectedObject = null;
                ChangeDepth(30);
            }
        }
        else
        {
            Ray ray = viewCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            bool detectInteract = Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactLayer);
            if (detectInteract && !inspectMode)
            {
                Interactable isInteractable = hit.collider.gameObject.GetComponentInChildren<Interactable>();
                if (isInteractable && !isInteractable.done)
                {
                    InteractableItem thisItem = hit.collider.gameObject.GetComponentInChildren<InteractableItem>();
                    InteractableSwitch thisSwitch = hit.collider.gameObject.GetComponentInChildren<InteractableSwitch>();
                    usableItem = isInteractable;
                    doorLocked = thisSwitch && thisSwitch.locked;

                    if (thisItem && !thisItem.done)
                    {
                        usableItem.HighLight(true);
                        inspectedObject = thisItem;
                        if (Input.GetMouseButtonDown(0))
                        {
                            inspectedObject.Inspect(transform);
                            inspectMode = true;
                            ChangeDepth(inspectedObject.offset);
                        }
                    }
                    else if (thisSwitch && !thisSwitch.locked)
                    {                        
                        usableItem.HighLight(true);
                        if (!thisSwitch.busy && Input.GetMouseButtonDown(0))
                        {
                            thisSwitch.Effect();
                        }
                    }
                }
            }
            else
            {
                doorLocked = false;

                if (usableItem)
                {
                    usableItem.HighLight(false);
                    usableItem = null;
                    inspectedObject = null;
                }
            }

            if (cursor)
            {
                cursor.Lock(doorLocked);
                cursor.gameObject.SetActive(detectInteract && !inspectMode);
            }
        }
    }

    Vector3 CalculateOffset(float t)
    {
        Vector3 offset = Vector3.zero;
        if (t > 0)
        {
            float horizontalOffset = Mathf.Cos(t * bobFrequency) * bobHorizontalAmplitude;
            float verticalOffset = Mathf.Sin(t * bobFrequency * 2) * bobVerticalAmplitude;
            offset = head.right * horizontalOffset + head.up * verticalOffset;
        }

        return offset;
    }

    void HeadBobbing()
    {
        targetCameraPosition = head.transform.position + CalculateOffset(walkingTime);
        viewCam.transform.position = Vector3.Lerp(viewCam.transform.position, targetCameraPosition, headBobSmoothing);
        if ((viewCam.transform.position - targetCameraPosition).magnitude <= 0.001f)
            viewCam.transform.position = targetCameraPosition;
    }

    private void Update()
    {
        if (!isDead)
        {
            Interact();

            if (!inspectMode)
            {
                FPS_Move();
                CameraControl();
                HeadBobbing();
            }
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
