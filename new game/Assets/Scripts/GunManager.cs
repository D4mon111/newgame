using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] float refreshRate = 0.5f;
    [SerializeField] KeyCode primaryFire = KeyCode.Mouse0;
    [SerializeField] float flashWidth = 0.1f;

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

            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[4]
            {
                cam.transform.position,
                cam.transform.position + (cam.transform.right * flashWidth),
                hit.point,
                hit.point + (cam.transform.right * flashWidth)
            };
            mesh.vertices = vertices;
            int[] tris = new int[6]
            {
                0, 1, 2,
                1, 3, 2
            };
            mesh.triangles = tris;
            Vector3[] normals = new Vector3[4]
            {
                Vector3.Cross(cam.transform.right, cam.transform.forward),
                Vector3.Cross(cam.transform.right, cam.transform.forward),
                Vector3.Cross(cam.transform.right, cam.transform.forward),
                Vector3.Cross(cam.transform.right, cam.transform.forward),
            };
            mesh.normals = normals;
            Vector2[] uv = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
            mesh.uv = uv;
            meshFilter.mesh = mesh;
        }
        timeSinceLastShot += Time.fixedDeltaTime;
    }
}
