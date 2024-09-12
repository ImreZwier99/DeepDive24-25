using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import the UI namespace for Slider

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
    [SerializeField] private LayerMask interactableLayerMask; // Layer mask for interactables
    [SerializeField] private GameObject paperPrefab; // Reference to the paper prefab
    [SerializeField] private float paperDistance;
    [SerializeField] private float paperRotationX;
    [SerializeField] private float paperXOffset; // X Offset for held paper
    [SerializeField] private float paperYoffset;

    [Header("Watering System")]
    [SerializeField] private Slider wateringSlider; // Slider UI element
    private float wateringCooldown; // Cooldown time between 45-60 seconds
    private bool canWaterPlant = false; // Flag to check if plant can be watered
    public float waterTimer; // Timer for watering cooldown

    private const float minWateringTime = 45f; // Minimum countdown time
    private const float maxWateringTime = 60f; // Maximum countdown time
    private const float minWateringThreshold = 10f; // Show slider and allow watering if timer is <= 10s

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

        // Set a random watering cooldown between 45 and 60 seconds
        ResetWateringTimer();

        // Set up the watering slider and initially hide it
        if (wateringSlider != null)
        {
            wateringSlider.gameObject.SetActive(false); // Hide slider at the start
        }
    }

    void Update()
    {
        HandleMouseLook();
        HandleInput();
        UpdateWateringTimer(); // Update the countdown timer every frame
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
            if (heldPaper == null && canGrab && paperStack != null && PaperStack.counter > 0)
            {
                GrabPaper();
            }
        }

        // Check for 'Q' key press to delete the held paper
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (heldPaper != null)
            {
                DeletePaper();
            }
        }

        // Water plant if looking at plant and countdown is <= 10 seconds
        if (Input.GetMouseButtonDown(0) && canWaterPlant && waterTimer <= minWateringThreshold)
        {
            WaterPlant();
        }
    }

    private void CheckRaycast()
    {
        // Perform a raycast from the camera's position
        Ray ray = new Ray(_camera.position, _camera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayerMask))
        {
            if (hit.collider.CompareTag("PaperStack"))
            {
                canGrab = true;
            }
            else
            {
                canGrab = false;
            }

            // Check if looking at a plant
            if (hit.collider.CompareTag("Plant"))
            {
                canWaterPlant = true;
            }
            else
            {
                canWaterPlant = false;
            }
        }
        else
        {
            canGrab = false;
            canWaterPlant = false;
        }
    }

    private void GrabPaper()
    {
        RandomizedPapers.OnNewSpawn();
        if (paperPrefab == null)
        {
            Debug.LogError("Paper Prefab is not assigned!");
            return;
        }

        // Instantiate the paper prefab in front of the camera
        Vector3 paperPosition = _camera.position + _camera.forward * paperDistance;
        Quaternion paperRotation = _camera.rotation * Quaternion.Euler(paperRotationX, 0, 0);
        paperPosition.x += paperXOffset; // Apply X offset
        paperPosition.y -= paperYoffset; // Apply Y offset

        heldPaper = Instantiate(paperPrefab, paperPosition, paperRotation);
        heldPaper.transform.parent = _camera; // Make the paper follow the camera

        // Decrease the counter in the PaperStack script
        if (paperStack != null)
        {
            paperStack.DecreaseCounter();
        }
    }

    private void DeletePaper()
    {
        if (heldPaper != null)
        {
            // Destroy the currently held paper
            Destroy(heldPaper);
            heldPaper = null; // Clear the reference
        }
    }

    private void UpdateWateringTimer()
    {
        if (waterTimer > 0)
        {
            waterTimer -= Time.deltaTime;

            // Show slider and update its value only when waterTimer is <= 10
            if (waterTimer <= minWateringThreshold)
            {
                wateringSlider.gameObject.SetActive(true);
                wateringSlider.maxValue = minWateringThreshold;
                wateringSlider.value = waterTimer;
            }
            else
            {
                // Hide the slider when waterTimer > 10
                wateringSlider.gameObject.SetActive(false);
            }
        }
    }

    private void WaterPlant()
    {
        // Reset the watering timer after watering the plant
        ResetWateringTimer();

        // Additional functionality for watering the plant can go here
        Debug.Log("Plant has been watered. Timer reset.");
    }

    private void ResetWateringTimer()
    {
        waterTimer = UnityEngine.Random.Range(minWateringTime, maxWateringTime);
        if (wateringSlider != null)
        {
            wateringSlider.gameObject.SetActive(false); // Hide slider until timer reaches 10
        }
    }
}
