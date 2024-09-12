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

    public static int[] dagen = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
    public static int[] maanden = { 1, 2, 3, 4, 5, 6 };

    public static int toegankelijkheid;
    public static int ondertekend;

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
        RandomizeName();
    }

    void Update()
    {
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

        string dag = dagen[random.Next(0,dagen.Length)].ToString();
    }

    IEnumerator WaitForSpawnedItem()
    {
        isSpawned = true;

        bool foundAllComponents = false;

        while (!foundAllComponents)
        {
            naamText = GameObject.FindGameObjectWithTag("naamtext")?.GetComponent<TextMeshProUGUI>();
            priveText = GameObject.FindGameObjectWithTag("toegankelijkheidstest")?.GetComponent<TextMeshProUGUI>();
            datumText = GameObject.FindGameObjectWithTag("datumText")?.GetComponent<TextMeshProUGUI>();
            handtekeningImage = GameObject.FindGameObjectWithTag("HandtekeningText");

            if (naamText != null && priveText != null && datumText != null && handtekeningImage != null)
            {
                foundAllComponents = true;
                UpdateUI();
            }

            yield return null;
        }

        isSpawned = false;
    }

    void UpdateUI()
    {
        naamText.text = fullName;

        if (privé)
        {
            priveText.text = "Privé";
        }
        else
        {
            priveText.text = "Openbaar";
        }

        if (toegankelijkheid <= 1)
        {
            privé = false;
        }
        else
        {
            privé = true;
        }

        if (ondertekend <= 1)
        {
            handTekening = false;
        }
        else
        {
            handTekening = true;
        }

        if (handTekening)
        {
            handtekeningImage.SetActive(true);
        }
        else
        {
            handtekeningImage.SetActive(false);
        }

        if (fullName == "Thomas Mulder" && handTekening && geldigeDatum)
        {
            belangrijk = true;
        }
        else
        {
            belangrijk = false;
        }
    }

    public static void OnNewSpawn()
    {
        RandomizeName();
        Randomizer();
        isSpawned = false;
    }

    public static void Randomizer()
    {
        var rnd = new System.Random();

        toegankelijkheid = rnd.Next(0, 3);
        ondertekend = rnd.Next(0, 3);
    }
}