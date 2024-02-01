using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed         = 2.5f;
    [SerializeField] float sprintSpeed   = 4.5f;
    [SerializeField] float slideSpeed    = 6f  ;
    [SerializeField] float jumpStrength  = 400f;
    [SerializeField] float groundControl = 1f  ;
    [SerializeField] float airControl    = 0.2f;
    [SerializeField] float MaxSpeed      = 35f ;
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
    bool slide = false;

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
        slide = Input.GetKey("left ctrl");
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
        return Physics.BoxCast(
            transform.position,
            new Vector3(0.45f, 0.45f, 0.45f),
            new Vector3(0, -1, 0),
            Quaternion.identity,
            1.1f
            );
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

        if (IsGrounded())
        {
            rb.drag = 5;
            if (jump) { rb.AddForce(new Vector3(0, jumpStrength * 100, 0) + rb.velocity * 0.2f, ForceMode.Force); }
        }
        else
        {
            rb.drag = 0;
        }
        if (slide)
        {
            rb.drag = 0;
            Vector3 direction = cam.transform.forward;
            direction.y = 0; //no slidey into the sky
            rb.AddForce(direction.normalized * slideSpeed, ForceMode.VelocityChange);
        }

        Vector3 force = new Vector3(0, 0, 0);
        float clamperY = Convert.ToSingle(2 * (1 / (1 + Math.Exp(-MaxSpeed + Vector3.Project(rb.velocity, cam.transform.forward).magnitude))));

        force += ((cam.transform.forward * yInput * clamperY) + (cam.transform.right * xInput));
        force = Vector3.ClampMagnitude(force, 1) * Mathf.Lerp(SprintSpeedLastUpdate, SprintSpeed, 0.8f);
        force *= (IsGrounded() ? groundControl : airControl);
        force.y = 0;
        rb.AddForce(force, ForceMode.VelocityChange);

        rb.AddForce(new Vector3(0, -gravity * 100, 0), ForceMode.Force);
    }
}
