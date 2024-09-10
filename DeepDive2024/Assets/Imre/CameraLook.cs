using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("Look Variables")]
    [SerializeField] private Transform hand;
    [SerializeField] private Transform _camera;
    [SerializeField] private float camSens = 200f;
    [SerializeField] private float camAcc = 5f;
    private float rotationX;
    private float rotationY;
    public float rotXClamp;
    public float rotYClamp;

    [Header("Raycast Variables")]
    [SerializeField] private float rayDistance = 10f; // Max ray distance
    [SerializeField] private LayerMask interactableLayerMask; // Add layer mask to limit raycast to specific layers if needed
    [SerializeField] private GameObject paperPrefab; // Reference to the paper prefab
    [SerializeField] private float paperDistance = 2f; // Distance from camera to instantiate the paper
    [SerializeField] private float paperRotationX = 20f; // Rotation on X-axis when the paper is grabbed
    [SerializeField] private float paperYoffset = 0.5f; // Offset on Y-axis for paper positioning
    [SerializeField] private float paperXOffset = 0.5f; // Offset on X-axis for paper positioning

    public GameObject heldPaper; // Track the currently held paper
    private PaperStack paperStack; // Reference to the PaperStack script
    private bool canGrab = false; // Flag to check if paper can be grabbed

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Find the PaperStack script in the scene
        paperStack = FindObjectOfType<PaperStack>();
        if (paperStack == null)
        {
            Debug.LogError("PaperStack script not found in the scene.");
        }
    }

    void Update()
    {
        HandleMouseLook();
        HandleInput();
        CheckRaycast();
    }

    private void HandleMouseLook()
    {
        // Handle mouse input for camera movement
        rotationX += Input.GetAxis("Mouse Y") * camSens * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse X") * camSens * Time.deltaTime;

        // Clamp the rotations
        rotationX = Mathf.Clamp(rotationX, -rotXClamp, rotXClamp);
        rotationY = Mathf.Clamp(rotationY, -rotYClamp, rotYClamp);

        // Rotate hand and camera
        hand.localRotation = Quaternion.Euler(-rotationX, rotationY, 0);
        transform.localRotation = Quaternion.Lerp(transform.localRotation,
            Quaternion.Euler(0, rotationY, 0), camAcc * Time.deltaTime);
        _camera.localRotation = Quaternion.Lerp(_camera.localRotation,
            Quaternion.Euler(-rotationX, 0, 0), camAcc * Time.deltaTime);
    }

    private void HandleInput()
    {
        // Check for left mouse button press
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            if (heldPaper == null && canGrab && paperStack != null && paperStack.counter > 0)
            {
                GrabPaper();
            }
        }
    }

    private void CheckRaycast()
    {
        // Perform a raycast from the camera's position
        Ray ray = new Ray(_camera.position, _camera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayerMask))
        {
            // Check if the object hit has the tag "PaperStack"
            if (hit.collider.CompareTag("PaperStack"))
            {
                canGrab = true; // You can grab paper when looking at this
            }
            else
            {
                canGrab = false;
            }
        }
        else
        {
            canGrab = false;
        }
    }

    private void GrabPaper()
    {
        if (paperPrefab == null)
        {
            Debug.LogError("Paper Prefab is not assigned!");
            return;
        }

        // Instantiate the paper prefab in front of the camera
        Vector3 paperPosition = _camera.position + _camera.forward * paperDistance; // Adjust the distance as needed

        // Apply rotation and position offset
        Quaternion paperRotation = _camera.rotation * Quaternion.Euler(paperRotationX, 0, 0); // Rotate paper on X axis
        paperPosition.y -= paperYoffset; // Offset the Y position downwards (adjust as needed)
        paperPosition.x += paperXOffset; // Offset the X position (adjust as needed)

        heldPaper = Instantiate(paperPrefab, paperPosition, paperRotation);
        heldPaper.transform.parent = _camera; // Make the paper follow the camera

        // Decrease the counter in the PaperStack script
        if (paperStack != null)
        {
            paperStack.DecreaseCounter();
        }

        Debug.Log("Paper grabbed and instantiated at: " + paperPosition);
    }

    private void DeletePaper()
    {
        if (heldPaper != null)
        {
            // Destroy the currently held paper
            Destroy(heldPaper);
            heldPaper = null; // Clear the reference

            Debug.Log("Paper deleted.");
        }
    }
}
