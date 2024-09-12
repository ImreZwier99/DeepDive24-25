using UnityEngine;
using UnityEngine.UI;

public class WateringSystem : MonoBehaviour
{
    [SerializeField] private Slider wateringSlider;
    private bool canWaterPlant = false;
    public float waterTimer;
    private const float minWateringTime = 45f;
    private const float maxWateringTime = 60f;
    private const float minWateringThreshold = 10f;

    private void Start()
    {
        ResetWateringTimer();
        if (wateringSlider != null)
        {
            wateringSlider.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        UpdateWateringTimer();
        HandleInput();
        CheckRaycast();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && canWaterPlant && waterTimer <= minWateringThreshold)
        {
            WaterPlant();
        }
    }

    private void CheckRaycast()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            canWaterPlant = hit.collider.CompareTag("Plant");
        }
        else
        {
            canWaterPlant = false;
        }
    }

    private void UpdateWateringTimer()
    {
        if (waterTimer > 0)
        {
            waterTimer -= Time.deltaTime;

            if (waterTimer <= minWateringThreshold)
            {
                wateringSlider.gameObject.SetActive(true);
                wateringSlider.maxValue = minWateringThreshold;
                wateringSlider.value = waterTimer;
            }
            else
            {
                wateringSlider.gameObject.SetActive(false);
            }
        }
    }

    private void WaterPlant()
    {
        ResetWateringTimer();
        Debug.Log("Plant has been watered. Timer reset.");
    }

    private void ResetWateringTimer()
    {
        waterTimer = Random.Range(minWateringTime, maxWateringTime);
        wateringSlider.gameObject.SetActive(false);
    }
}
