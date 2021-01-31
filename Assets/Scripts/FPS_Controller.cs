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
    [SerializeField] Image cursor;
    Rigidbody rb;
    bool isDead;
    PostProcessVolume volume;

    [Header("Movements")]
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float camSensitivity = 150;
    [SerializeField] float camBounds = 40;
    [SerializeField] [Range(0,1)] float walkCadency = 1f;
    float rotY;
    float timer;

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
        viewCam = Camera.main;
        rb = GetComponent<Rigidbody>();
        volume = FindObjectOfType<PostProcessVolume>();
    }

    private void Start()
    {
        EventManager.instance.onEndGame.AddListener(StopPlayer);
        EventManager.instance.onGameStart.Invoke();
    }

    void StopPlayer()
    {
        //isDead = true;
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
            timer += Time.deltaTime;
            if (timer > walkCadency)
            {
                timer = 0;
                SoundManager.instance.RandomStep(transform);
            }
        }
        else
            timer = walkCadency/2;
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
                    usableItem.HighLight(true);

                    if (thisItem)
                    {
                        inspectedObject = thisItem;
                        if (Input.GetMouseButtonDown(0))
                        {
                            inspectedObject.Inspect(transform);
                            inspectMode = true;
                            ChangeDepth(inspectedObject.offset);
                        }
                    }
                    else if (thisSwitch)
                    {
                        if (!thisSwitch.busy && Input.GetMouseButtonDown(0))
                        {
                            thisSwitch.Effect();
                        }
                    }
                }
            }
            else
            {
                if (usableItem)
                {
                    usableItem.HighLight(false);
                    usableItem = null;
                    inspectedObject = null;
                }
            }

            if (cursor)
                cursor.gameObject.SetActive(detectInteract && !inspectMode);
        }
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (!isDead)
        {
            Interact();

            if (!inspectMode)
            {
                FPS_Move();
                CameraControl();
            }
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }
}
