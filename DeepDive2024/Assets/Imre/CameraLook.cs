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
    [SerializeField] private PaperStack paperStack; // Reference to the PaperStack script

    private bool isLookingAtInteractable = false; // Track if the raycast is currently hitting an interactable object

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        CheckRaycast();
        HandleInput();
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

    private void CheckRaycast()
    {
        // Raycast from the center of the screen
        Ray ray = new Ray(_camera.position, _camera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayerMask))
        {
            // Check if the hit object has the tag "interactable"
            if (hit.collider.CompareTag("interactable"))
            {
                isLookingAtInteractable = true;
                Debug.Log("Hit interactable object: " + hit.collider.gameObject.name);
            }
            else
            {
                isLookingAtInteractable = false;
            }
        }
        else
        {
            isLookingAtInteractable = false;
        }
    }

    private void HandleInput()
    {
        // Check for left mouse button press
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            if (isLookingAtInteractable)
            {
                // Decrease the counter in the PaperStack component
                if (paperStack != null)
                {
                    paperStack.DecreaseCounter();
                }
            }
        }
    }
}
