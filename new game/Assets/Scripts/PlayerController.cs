using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 30f;
    [SerializeField] float sprintSpeed = 2f;
    [SerializeField] float jumpStrength = 900f;
    [SerializeField] float MaxSpeed = 35f;
    [Header("Input")]
    [SerializeField] float sensitivity = 20f;
    [SerializeField] float maxLookUp = 90f;
    [SerializeField] float maxLookDown = -90f;
    [Header("Gameplay")]
    [SerializeField] float gravity = 40f;
    [Header("Permissions")]
    [SerializeField] bool movable = true;
    [SerializeField] bool rotatable = true;


    float XRotation = 0f;
    float YRotation = 0f;

    Rigidbody rb;
    GameObject cam;
    float xInput = 0f;
    float yInput = 0f;
    bool jump = false;
    bool sprint = false;

    float SprintSpeed = 0f;
    float SprintSpeedLastUpdate = 0f;

    // Start is called before the first frame update
    void Start()
    {
        cam = transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        jump = Input.GetKey("space");
        sprint = Input.GetKey("left shift");
        if (rotatable) 
        {
            RotationHandler();
        }
    }
    private void FixedUpdate()
    {
        if (movable)
        {
            MovementHandler();
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position - new Vector3(0, .99f, 0), -Vector3.up, 0.3f);
    }

    public void RotationHandler()
    {
        float mousex = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity * 100;
        float mousey = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity * 100;

        XRotation += mousex;
        YRotation += mousey;
        YRotation = Mathf.Clamp(YRotation, maxLookDown, maxLookUp);
        cam.transform.rotation = Quaternion.Euler(new Vector3(-YRotation, XRotation, 0));
    }
    public void MovementHandler()
    {
        SprintSpeed = (sprint ? sprintSpeed : speed);
        SprintSpeedLastUpdate = SprintSpeed;

        Vector3 force = new Vector3(0, 0, 0);

        force += ((cam.transform.forward * yInput) + (cam.transform.right * xInput)) * speed * 0.01f * Mathf.Lerp(SprintSpeedLastUpdate, SprintSpeed, 0.8f) * (IsGrounded() ? 0.8f : 0.2f);
        force = Vector3.ClampMagnitude(force, 1);
        force.y = 0;
        rb.AddForce(force, ForceMode.VelocityChange);

        rb.AddForce(new Vector3(0, -gravity * 100, 0), ForceMode.Force);
        if (jump && IsGrounded())
        {
            rb.AddForce(new Vector3(0, jumpStrength * 100, 0) + rb.velocity * 0.2f, ForceMode.Force);
        }
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSpeed);
    }
}
