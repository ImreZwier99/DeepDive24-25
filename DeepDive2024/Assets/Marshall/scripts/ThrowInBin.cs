using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThrowInBin : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private LayerMask interactableLayerMask;
    public GameObject Papierpropje;
    public GameObject propThrow;
    public bool lookingToBin = false;

    void Start()
    {
        lookingToBin = false;
    }

    void Update()
    {
        SendRaycast();

        if (lookingToBin && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(Papierpropje, propThrow.transform.position, propThrow.transform.rotation);
        }
    }

    private void SendRaycast()
    {
        Ray ray = new Ray(_camera.position, _camera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayerMask))
        {
            print(hit.collider.gameObject);
            if (hit.collider.CompareTag("Bin"))
            {
                lookingToBin = true;
                print("looking at");
            }
        }
        else
        {
            print("looking away");
            lookingToBin= false;
        }
    }
}
