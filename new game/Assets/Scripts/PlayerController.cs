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
    [SerializeField] float speed = 1f;
    [SerializeField] float sprintSpeed = 2f;
    [SerializeField] float jumpStrength = 700f;
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
    float xInput = 0f;
    float yInput = 0f;
    bool jump = false;
    bool sprint = false;

    float SprintSpeed = 0f;
    float SprintSpeedLastUpdate = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.transform.parent.GetComponent<Rigidbody>();
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
        return Physics.Raycast(transform.position - new Vector3(0, 1.7f, 0), -Vector3.up, 0.3f);
    }

    public void RotationHandler()
    {
        float mousex = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity * 100;
        float mousey = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity * 100;

        XRotation += mousex;
        YRotation += mousey;
        YRotation = Mathf.Clamp(YRotation, maxLookDown, maxLookUp);
        transform.rotation = Quaternion.Euler(new Vector3(-YRotation, XRotation, 0));
    }
    public void MovementHandler()
    {
        SprintSpeed = (sprint ? sprintSpeed : speed);
        SprintSpeedLastUpdate = SprintSpeed;
        float vy = rb.velocity.y;
        rb.velocity = ((transform.forward * yInput) + (transform.right * xInput)) * Mathf.Lerp(SprintSpeedLastUpdate, SprintSpeed, 0.8f) * 10;
        rb.velocity = new Vector3(rb.velocity.x, vy, rb.velocity.z);
        rb.AddForce(new Vector3(0, -gravity * 100, 0), ForceMode.Force);
        if (jump && IsGrounded())
        {
            rb.AddForce(new Vector3(0, jumpStrength * 100, 0), ForceMode.Force);
        }
    }
}
