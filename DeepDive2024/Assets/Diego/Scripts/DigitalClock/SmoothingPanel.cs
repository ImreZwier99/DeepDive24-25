using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SmoothingPanel : MonoBehaviour
{
    public bool smooth;
    public static bool smoothing;
    private bool isActive = false;

    public GameObject detailPanel;
    private float panelTimer = 15;
    public Animator smoothingPanel_Animator;
    public TextMeshProUGUI timer_Text;

    void Update()
    {
		if (isActive)
		{
            DetailPanelBehaviour();
            timer_Text.text = Mathf.Round(panelTimer).ToString() + " | Enter";
        }
        smoothing = smooth;
    }

    public void DetailPanelAvtivation()
	{
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
