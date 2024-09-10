using System;
using TMPro;
using UnityEngine;

public class DigitalClock : MonoBehaviour
{
    public TextMeshProUGUI clockText; // De klokweergave
    public TextMeshProUGUI dayText;   // De dagweergave

    private TimeSpan startTime = new TimeSpan(8, 57, 0);  // Starttijd om 08:57
    private TimeSpan endTime = new TimeSpan(17, 0, 0);    // Eindtijd om 17:00
    private TimeSpan currentTime;                         // Huidige in-game tijd
    public float timeSpeed = 2.0f;                       // 1 minuut in-game duurt 2 seconden in real life
    private float timeCounter = 0f;                       // Timer om de tijd te tracken
    private int dayIndex = 0;                             // Start op maandag (index voor de dagen)

    private string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

    void Start()
    {
        // Stel de initiële tijd en dag in
        currentTime = startTime;
        dayText.text = days[dayIndex];
        UpdateClock();
    }

    void Update()
    {
        // Tel de echte tijd (in seconden) op
        timeCounter += Time.deltaTime;

        // Controleer of er 2 seconden zijn verstreken om 1 minuut in-game te simuleren
        if (timeCounter >= timeSpeed)
        {
            // Verhoog de in-game tijd met 1 minuut
            currentTime = currentTime.Add(new TimeSpan(0, 1, 0));
            timeCounter = 0f;

            // Controleer of de tijd het eindpunt van 17:00 heeft bereikt
            if (currentTime >= endTime)
            {
                // Zet de tijd terug naar 08:57
                currentTime = startTime;

                // Ga naar de volgende dag
                dayIndex++;

                // Als we voorbij vrijdag zijn, terug naar maandag
                if (dayIndex >= days.Length)
                {
                    dayIndex = 0;
                }

                // Update de dagweergave
                dayText.text = days[dayIndex];
            }

            // Update de klokweergave
            UpdateClock();
        }
    }

    // Functie om de klokweergave te updaten
    void UpdateClock()
    {
        // Format de tijd als hh:mm
        clockText.text = currentTime.ToString(@"hh\:mm");
    }
}
