using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomizedPapers : MonoBehaviour
{
    private static string[] voornamen = { "Thomas", "Tess", "Sara", "Max", "Luuk", "Anna", "Noah", "Milan", "Lotte", "Emma" };
    private static string[] achternamen = { "Visser", "De Jong", "De Boer", "Vos", "Mulder", "Smit", "Bakker", "Dijkstra", "Jansen", "Meijer" };

    public static string fullName;
    public static string datum;

    public static int toegankelijkheid;
    public int ondertekend;

    public static bool handTekening = false;
    public static bool privé = false;
    public static bool geldigeDatum = true;
    public static bool belangrijk = false;

    public TextMeshProUGUI naamText;
    public TextMeshProUGUI priveText;
    public TextMeshProUGUI datumText;
    public GameObject handtekeningImage;

    private static bool isSpawned = false;

    void Start()
    {
        // Randomize name on start
        RandomizeName();
    }

    void Update()
    {
        // Continuously check for new spawns (can also be triggered by an event or method)
        if (!isSpawned)
        {
            StartCoroutine(WaitForSpawnedItem());
        }
    }

    public static void RandomizeName()
    {
        var random = new System.Random();
        string firstName = voornamen[random.Next(0, voornamen.Length)];
        string lastName = achternamen[random.Next(0, achternamen.Length)];
        fullName = firstName + " " + lastName;
    }

    IEnumerator WaitForSpawnedItem()
    {
        isSpawned = true;  // Temporarily mark as spawned until we reset it later

        // Continuously check until all necessary components are found
        bool foundAllComponents = false;

        while (!foundAllComponents)
        {
            // Find UI components dynamically
            naamText = GameObject.FindGameObjectWithTag("naamtext")?.GetComponent<TextMeshProUGUI>();
            priveText = GameObject.FindGameObjectWithTag("toegankelijkheidstest")?.GetComponent<TextMeshProUGUI>();
            datumText = GameObject.FindGameObjectWithTag("datumText")?.GetComponent<TextMeshProUGUI>();
            handtekeningImage = GameObject.FindGameObjectWithTag("HandtekeningText");

            // Check if all components have been found
            if (naamText != null && priveText != null && datumText != null && handtekeningImage != null)
            {
                foundAllComponents = true;
                UpdateUI();  // Update UI after components have been found
            }

            yield return null;  // Wait for next frame before rechecking
        }

        // Reset after UI is updated, so that it can detect new spawns in the next frame
        isSpawned = false;  // Allow re-triggering when a new spawn occurs
    }

    void UpdateUI()
    {
        // Update the UI text and visibility
        naamText.text = fullName;

        if (privé)
        {
            priveText.text = "Privé";
        }
        else
        {
            priveText.text = "Openbaar";
        }

        if (handTekening)
        {
            handtekeningImage.SetActive(true);
        }
        else
        {
            handtekeningImage.SetActive(false);
        }

        // Check if the document is "belangrijk"
        if (fullName == "Thomas Mulder" && handTekening && geldigeDatum)
        {
            belangrijk = true;
        }
        else
        {
            belangrijk = false;
        }

        // Optionally trigger any post-UI update logic here (like spawning new papers).
    }

    public static void OnNewSpawn()
    {
        // Call this method every time an item is spawned to reset and trigger the randomization and UI update
        RandomizeName();
        isSpawned = false;  // Force the script to recheck for UI components and update them
    }

    void Randomizer()
    {
        var rnd = new System.Random();

        // Randomize toegankelijkheid and ondertekend status
        toegankelijkheid = rnd.Next(0, 3);
        ondertekend = rnd.Next(0, 3);
    }
}