using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapierPropje : MonoBehaviour
{
    public Rigidbody rb;
    public float force = 10f;
    public Camera cam;
    public float destroyTimer = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (cam == null)
        {
            cam = Camera.main;
        }

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 direction = ray.direction;

        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    void Update()
    {
        destroyTimer -= Time.deltaTime;

        if (destroyTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
