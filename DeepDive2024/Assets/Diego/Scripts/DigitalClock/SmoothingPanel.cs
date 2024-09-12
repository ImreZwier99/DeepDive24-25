using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SmoothingPanel : MonoBehaviour
{
    public bool smooth;
    public static bool smoothing, completedDay = true;
    private bool isActive = false;

    public GameObject detailPanel, completedDayText, failedDayButtons;
    private float panelTimer = 15;
    public Animator smoothingPanel_Animator;
    public TextMeshProUGUI timer_Text, headText;

	private void Start()
	{
        completedDay = true;
	}

	void Update()
    {
		if (isActive && completedDay && DigitalClock.dayIndex != 4)
		{
            DetailPanelBehaviour();
            timer_Text.text = Mathf.Round(panelTimer).ToString() + " | Enter";
        }
        smoothing = smooth;
    }

    public void DetailPanelActivation()
	{
		if (completedDay && DigitalClock.dayIndex != 4)
		{
            completedDayText.SetActive(true);
            failedDayButtons.SetActive(false);
            headText.text = "Day completed";
            headText.color = Color.green;
		}
        else if(completedDay && DigitalClock.dayIndex == 4)
		{
            completedDayText.SetActive(false);
            failedDayButtons.SetActive(true);
            headText.text = "Week completed";
            headText.color = Color.green;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (!completedDay)
		{
            completedDayText.SetActive(false);
            failedDayButtons.SetActive(true);
            headText.text = "Day failed";
            headText.color= Color.red;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
		}

        detailPanel.SetActive(true);
        isActive = true;
    }

    private void DetailPanelBehaviour()
	{
        panelTimer -= Time.deltaTime;
        if(panelTimer <= 0 || Input.GetKeyDown(KeyCode.Return))
		{
            panelTimer = 15;
            smoothingPanel_Animator.SetBool("Smoothing", false);
            detailPanel.SetActive(false);
            isActive = false;
        }
	}
}
