using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FPS_Controller : MonoBehaviour
{
    Camera viewCam;
    Rigidbody rb;
    bool isDead;

    [Header("Movements")]
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float camSensitivity = 150;
    [SerializeField] float camBounds = 40;
    float rotY;

    private void Awake()
    {
        viewCam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void FPS_Move()
    {
        Vector3 horizontalAxis = transform.right * Input.GetAxisRaw("Horizontal");
        Vector3 verticalAxis = transform.forward * Input.GetAxisRaw("Vertical");
        Vector3 normalized = (horizontalAxis + verticalAxis).normalized;

        Vector3 move = normalized * moveSpeed;
        move.y = 0;
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
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

    private void Update()
    {
        if (!isDead)
        {
            FPS_Move();
            CameraControl();
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        Cursor.lockState = CursorLockMode.Locked;
    }
}
