using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] float refreshRate = 0.5f;
    [SerializeField] KeyCode primaryFire = KeyCode.Mouse0;
    [SerializeField] float flashWidth = 0.1f;
    [SerializeField] Vector3 gunoffset;

    float timeSinceLastShot = 0;
    GameObject gun;
    GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = transform.GetChild(0).gameObject;
        gun = transform.GetChild(0).GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(primaryFire) && timeSinceLastShot > refreshRate)
        {
            timeSinceLastShot = 0;
            Ray dir = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;
            Physics.Raycast(dir, out hit);
            Debug.Log(hit.collider);
        }
        timeSinceLastShot += Time.fixedDeltaTime;
    }
}
