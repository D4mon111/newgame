using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] float refreshRate = 0.5f;

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
        if (Input.GetMouseButtonDown(0) && (timeSinceLastShot > refreshRate)) 
        {
            Ray dir = new Ray(gun.transform.position, cam.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(dir, out hit)) 
            {
                timeSinceLastShot = 0;
                //Debug.Log(Convert.ToString(hit.collider));
            }
        }
        Debug.Log(Convert.ToString(timeSinceLastShot) + "/" + Convert.ToString(refreshRate));
        timeSinceLastShot = Time.fixedTime;
    }
}
