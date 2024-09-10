using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPropje : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private LayerMask interactableLayerMask;
    public GameObject Papierpropje; // The crumpled paper prefab to throw
    public GameObject propThrow; // The position from where to throw
    public bool lookingToBin = false;

    // Reference to check if the player is holding a paper
    private CameraLook cameraLook; // Reference to CameraLook script to check heldPaper status

    void Start()
    {
        lookingToBin = false;

        // Find the CameraLook script
        cameraLook = FindObjectOfType<CameraLook>();
        if (cameraLook == null)
        {
            Debug.LogError("CameraLook script not found in the scene.");
        }
    }

    void Update()
    {
        SendRaycast();

        // Check if looking at the bin and the player clicks to throw, while holding a paper
        if (lookingToBin && Input.GetKeyDown(KeyCode.Mouse0) && cameraLook.heldPaper != null)
        {
            // Instantiate the crumpled paper (Papierpropje) at the propThrow position
            Instantiate(Papierpropje, propThrow.transform.position, propThrow.transform.rotation);

            // Delete the held paper
            DeleteHeldPaper();
        }
    }

    private void SendRaycast()
    {
        Ray ray = new Ray(_camera.position, _camera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayerMask))
        {
            if (hit.collider.CompareTag("Bin"))
            {
                lookingToBin = true;
            }
            else
            {
                lookingToBin = false;
            }
        }
        else
        {
            lookingToBin = false;
        }
    }

    private void DeleteHeldPaper()
    {
        // Check if there's a held paper in the CameraLook script
        if (cameraLook.heldPaper != null)
        {
            // Destroy the held paper
            Destroy(cameraLook.heldPaper);

            // Clear the reference to the held paper in CameraLook
            cameraLook.heldPaper = null;
        }
    }
}
