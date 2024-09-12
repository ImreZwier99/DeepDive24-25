using System;
using TMPro;
using UnityEngine;

public class DigitalClock : MonoBehaviour
{
    public TextMeshProUGUI clockText;    // De klokweergave
    public TextMeshProUGUI dayText;      // De dagweergave
    public TextMeshProUGUI dateText;     // De datumweergave

    private TimeSpan startTime = new TimeSpan(8, 57, 0);   // Starttijd om 08:57
    private TimeSpan endTime = new TimeSpan(17, 0, 0);     // Eindtijd om 17:00
    private TimeSpan currentTime;                          // Huidige in-game tijd
    public float timeSpeed = 2.0f;                         // 1 minuut in-game duurt 2 seconden in real life
    public static float timeCounter = 0f;                        // Timer om de tijd te tracken
    public static int dayIndex = 0;                              // Start op maandag (index voor de dagen)
    public Animator smoothingPanel_Animator;               // Animator component

    private string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
    private DateTime startDate = new DateTime(2024, 2, 10); // Stel een startdatum in
    private bool clockStopped = false;                     // Houdt bij of de klok is gestopt

    void Start()
    {
        // Stel de initiële tijd en dag in
        dayIndex = 0;
        currentTime = startTime;
        dayText.text = days[dayIndex];
        UpdateClock();
        UpdateDate(); // Initieel de datum bijwerken
    }

    void Update()
    {
        if (clockStopped)
        {
            // Controleer of de animatie is afgelopen door te kijken naar SmoothingPanel.smoothing
            if (SmoothingPanel.smoothing == false)  // Als de static bool 'smoothing' false is
            {
                smoothingPanel_Animator.SetBool("Smoothing", false);

                // Reset de tijd en dag na het afronden van de animatie
                ResetTimeAndAdvanceDay();
            }

            return; // Stop verdere updates totdat de animatie klaar is
        }

        timeCounter += Time.deltaTime;

        if (timeCounter >= timeSpeed)
        {
            currentTime = currentTime.Add(new TimeSpan(0, 1, 0));
            timeCounter = 0f;

            if (currentTime >= endTime)
            {
                // Stop de klok en start de animatie
                StartAnimation();
            }

            UpdateClock();
        }
    }

    void StartAnimation()
    {
        // Zet de animatiebool in de animator aan
        smoothingPanel_Animator.SetBool("Smoothing", true);
        clockStopped = true; // Stop de klok
    }

    void UpdateClock()
    {
        clockText.text = currentTime.ToString(@"hh\:mm");
    }

    void UpdateDate()
    {
        // Bereken de datum op basis van de dagindex
        DateTime currentDate = startDate.AddDays(dayIndex);
        // Format de datum als "MM/dd/yyyy" in cijfers
        string formattedDate = currentDate.ToString("dd/MM/yyyy");
        dateText.text = formattedDate;
    }

    void ResetTimeAndAdvanceDay()
    {
        currentTime = startTime;
        dayIndex++;
        AnnoyingMan.isActive = false;

        dayText.text = days[dayIndex];
        UpdateClock();
        UpdateDate(); // Werk de datum bij na het aanpassen van de dag
        clockStopped = false; // Klok opnieuw starten voor de volgende cyclus
    }
}
