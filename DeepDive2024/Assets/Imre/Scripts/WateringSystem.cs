using UnityEngine;
using UnityEngine.UI;

public class WateringSystem : MonoBehaviour
{
    [SerializeField] private Slider wateringSlider1; 
    [SerializeField] private Slider wateringSlider2; 
    private bool canWaterPlant1 = false;
    private bool canWaterPlant2 = false;

    public float waterTimer1;
    public float waterTimer2;

    private const float minWateringTime = 30f;
    private const float maxWateringTime = 60f;
    private const float minWateringThreshold = 10f;

    private void Start()
    {
        ResetWateringTimer(1);
        ResetWateringTimer(2);

        if (wateringSlider1 != null)
            wateringSlider1.gameObject.SetActive(false);

        if (wateringSlider2 != null)
            wateringSlider2.gameObject.SetActive(false);
    }

    void Update()
    {
        UpdateWateringTimer(1);
        UpdateWateringTimer(2);
        HandleInput();
        CheckRaycast();
        if (AnnoyingMan.isActive == true)
        {
            if(waterTimer1 <= 0 || waterTimer2 <= 0)
            {
                Debug.Log("you dead");
            }
            
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && canWaterPlant1 && waterTimer1 <= minWateringThreshold)
        {
            WaterPlant(1);
        }

        if (Input.GetMouseButtonDown(0) && canWaterPlant2 && waterTimer2 <= minWateringThreshold)
        {
            WaterPlant(2);
        }
    }

    private void CheckRaycast()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            canWaterPlant1 = hit.collider.CompareTag("Plant1");

            canWaterPlant2 = hit.collider.CompareTag("Plant2");
        }
        else
        {
            canWaterPlant1 = false;
            canWaterPlant2 = false;
        }
    }

    private void UpdateWateringTimer(int plantNumber)
    {
        if (plantNumber == 1 && waterTimer1 > 0)
        {
            waterTimer1 -= Time.deltaTime;

            if (waterTimer1 <= minWateringThreshold)
            {
                wateringSlider1.gameObject.SetActive(true);
                wateringSlider1.maxValue = minWateringThreshold;
                wateringSlider1.value = waterTimer1;
            }
            else
            {
                wateringSlider1.gameObject.SetActive(false);
            }
        }

        if (plantNumber == 2 && waterTimer2 > 0)
        {
            waterTimer2 -= Time.deltaTime;

            if (waterTimer2 <= minWateringThreshold)
            {
                wateringSlider2.gameObject.SetActive(true);
                wateringSlider2.maxValue = minWateringThreshold;
                wateringSlider2.value = waterTimer2;
            }
            else
            {
                wateringSlider2.gameObject.SetActive(false);
            }
        }
    }

    private void WaterPlant(int plantNumber)
    {
        if (plantNumber == 1)
        {
            ResetWateringTimer(1);
        }

        if (plantNumber == 2)
        {
            ResetWateringTimer(2);
        }
    }

    private void ResetWateringTimer(int plantNumber)
    {
        if (plantNumber == 1)
        {
            waterTimer1 = Random.Range(minWateringTime, maxWateringTime);
            wateringSlider1.gameObject.SetActive(false);
        }

        if (plantNumber == 2)
        {
            waterTimer2 = Random.Range(minWateringTime, maxWateringTime);
            wateringSlider2.gameObject.SetActive(false);
        }
    }
}
