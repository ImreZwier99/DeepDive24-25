using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehaviour : MonoBehaviour
{
    // De afstand waarbinnen de raycast objecten detecteert
    public float raycastRange = 100f;

    // Reference naar de camera (normaal gezien de main camera)
    public Camera playerCamera;

    // Lijst met tags waarmee we willen vergelijken
    public string[] binderTags = { "Binder", "Binder2", "Binder3", "Binder4", "Binder5" };  // Binder tags
    public string[] plantTags = { "Plant1", "Plant2" };  // Plant tags

    // Het huidige geselecteerde object
    private GameObject currentObject;

    // Referenties naar de twee cursor objecten
    public GameObject Cursor1Object;
    public GameObject Cursor2Object;

    // Variabele om de vorige waarde van isHoldingPaper bij te houden
    private bool previousHoldingPaperStatus;

    // Start is called before the first frame update
    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main; // Gebruik de main camera als er geen is toegewezen
        }

        // Zorg dat alleen Cursor1Object zichtbaar is bij de start
        if (Cursor1Object != null && Cursor2Object != null)
        {
            Cursor1Object.SetActive(true);  // Start met Cursor1 actief
            Cursor2Object.SetActive(false); // Cursor2 uitgeschakeld
        }

        // Zet de initiële waarde van de vorige holding paper status
        previousHoldingPaperStatus = PaperInteraction.isHoldingPaper;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition); // Creëer een ray vanaf de muispositie
        RaycastHit hit;

        // Controleer of de ray een object raakt binnen het bereik
        if (Physics.Raycast(ray, out hit, raycastRange))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Als het object hetzelfde is als het vorige, update alleen de cursor status
            if (hitObject == currentObject)
            {
                // Check of de PaperInteraction status is veranderd
                if (PaperInteraction.isHoldingPaper != previousHoldingPaperStatus)
                {
                    // Update de cursor als de status is veranderd
                    OnCursorEnter(currentObject);
                    previousHoldingPaperStatus = PaperInteraction.isHoldingPaper;
                }
            }
            else
            {
                // Als we naar een ander object kijken dan het vorige
                if (currentObject != null)
                {
                    OnCursorExit(currentObject); // Voer exit gedrag uit voor het vorige object
                }

                currentObject = hitObject;
                OnCursorEnter(currentObject); // Voer enter gedrag uit voor het nieuwe object

                // Update de vorige holding paper status
                previousHoldingPaperStatus = PaperInteraction.isHoldingPaper;
            }
        }
        else
        {
            // Als er niets wordt geraakt, en er was een object geselecteerd
            if (currentObject != null)
            {
                OnCursorExit(currentObject); // Verlaat het huidige object
                currentObject = null;
            }
        }
    }

    // Wat gebeurt er wanneer de cursor een object binnenkomt
    void OnCursorEnter(GameObject obj)
    {
        Debug.Log("Cursor over: " + obj.name);

        // Controleer of de tag 'PaperStack' is
        if (obj.CompareTag("PaperStack"))
        {
            if (!PaperInteraction.isHoldingPaper)
            {
                // Als je geen papier vasthoudt, wissel naar Cursor2
                SwitchToCursor2();
            }
            else
            {
                // Als je wel papier vasthoudt, wissel niet naar Cursor2
                Debug.Log("Cannot switch to Cursor2, holding paper");
                SwitchToCursor1();
            }
            return;
        }

        // Controleer of we naar een binder kijken en PaperInteraction.isHoldingPaper == true
        foreach (string tag in binderTags)
        {
            if (obj.CompareTag(tag) && PaperInteraction.isHoldingPaper)
            {
                SwitchToCursor2(); // Wissel naar Cursor2 als de speler papier vasthoudt en naar een binder kijkt
                return;
            }
        }

        // Controleer of we naar Plant1 kijken en canWaterPlant1 == true
        if (obj.CompareTag("Plant1") && WateringSystem.canWaterPlant1)
        {
            SwitchToCursor2();
            return;
        }

        // Controleer of we naar Plant2 kijken en canWaterPlant2 == true
        if (obj.CompareTag("Plant2") && WateringSystem.canWaterPlant2)
        {
            SwitchToCursor2();
            return;
        }

        // Als geen van de condities overeenkomt, ga terug naar Cursor1
        SwitchToCursor1();
    }

    // Wat gebeurt er wanneer de cursor een object verlaat
    void OnCursorExit(GameObject obj)
    {
        Debug.Log("Cursor verlaat: " + obj.name);
        SwitchToCursor1(); // Wissel terug naar Cursor1 bij het verlaten van een object
    }

    // Functie om naar Cursor1 te wisselen
    void SwitchToCursor1()
    {
        if (Cursor1Object != null && Cursor2Object != null)
        {
            Cursor1Object.SetActive(true);
            Cursor2Object.SetActive(false);
        }
    }

    // Functie om naar Cursor2 te wisselen
    void SwitchToCursor2()
    {
        if (Cursor1Object != null && Cursor2Object != null)
        {
            Cursor1Object.SetActive(false);
            Cursor2Object.SetActive(true);
        }
    }
}
