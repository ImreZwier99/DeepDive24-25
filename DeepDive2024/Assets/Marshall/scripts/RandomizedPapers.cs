using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class RandomizedPapers : MonoBehaviour
{
    private string[] voornamen = { "Thomas", "Tess", "Sara", "Max", "Luuk", "Anna", "Noah", "Milan", "Lotte", "Emma" };
    private string[] achternamen = { "Visser", "De Jong", "De Boer", "Vos", "Mulder", "Smit", "Bakker", "Dijkstra", "Jansen", "Meijer" };
    public string fullName;
    public string datum;

    public int toegankelijkheid;
    public int ondertekend;

    public bool handTekening = false;
    public bool privé = false;
    public bool geldigeDatum = true;
    public bool belangrijk = false;

    public TextMeshProUGUI naamText;
    public TextMeshProUGUI priveText;
    public GameObject handtekeningImage;

    void Start()
    {

        var random = new System.Random();
        string firstName = voornamen[random.Next(0, voornamen.Length)];
        string lastName = achternamen[random.Next(0, achternamen.Length)];
        fullName = firstName + " " + lastName;
        print(fullName);
        print(handTekening);
        print(privé);
    }

    void Update()
    {
        print("update werkt");
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

        if (fullName == "Thomas Mulder" && handTekening && geldigeDatum)
        {
            belangrijk = true;
        }
        else
        {
            belangrijk = false;
        }
    }

    void Randomizer()
    {
        var rnd = new System.Random();

        toegankelijkheid = rnd.Next(0,3);
        ondertekend = rnd.Next(0, 3);
    }
}
