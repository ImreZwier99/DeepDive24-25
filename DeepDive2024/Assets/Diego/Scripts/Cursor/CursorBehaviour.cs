using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehaviour : MonoBehaviour
{
    public float raycastRange = 100f;  // De afstand waarbinnen de raycast objecten detecteert
    public Camera playerCamera;        // De camera voor de raycast
    public string[] binderTags = { "Binder", "Binder2", "Binder3", "Binder4", "Binder5" };  // Binder tags
    private GameObject currentObject;  // Het huidige geselecteerde object
    public GameObject Cursor1Object;   // Eerste cursor
    public GameObject Cursor2Object;   // Tweede cursor

    private bool previousHoldingPaperStatus; // Vorige status van isHoldingPaper
    private bool isRaycastHitValid;    // Variabele om de raycast-validatie op te slaan

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        if (Cursor1Object != null && Cursor2Object != null)
        {
            Cursor1Object.SetActive(true);
            Cursor2Object.SetActive(false);
        }

        previousHoldingPaperStatus = PaperInteraction.isHoldingPaper;
    }

    void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Controleer de raycast en check of de ray een object raakt
        isRaycastHitValid = Physics.Raycast(ray, out hit, raycastRange);

        if (isRaycastHitValid)
        {
            GameObject hitObject = hit.collider.gameObject;

            // Als het object verandert of als de status van PaperInteraction verandert
            if (hitObject != currentObject || PaperInteraction.isHoldingPaper != previousHoldingPaperStatus)
            {
                if (currentObject != null)
                {
                    OnCursorExit(currentObject);  // Voer exit gedrag uit voor het vorige object
                }

                currentObject = hitObject;      // Update naar het nieuwe object
                OnCursorEnter(currentObject);        // Voer enter gedrag uit voor het nieuwe object

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

        // Controleer ook de status van de timers voor het huidige object
        if (currentObject != null)
        {
            CheckCursorStatusForCurrentObject(currentObject);
        }
    }

    // Functie voor als de cursor een object binnenkomt of de status verandert
    void OnCursorEnter(GameObject obj)
    {
        Debug.Log("Cursor over: " + obj.name);

        // Controleer of de tag 'PaperStack' is en of je geen papier vasthoudt
        if (obj.CompareTag("PaperStack"))
        {
            if (!PaperInteraction.isHoldingPaper)
            {
                SwitchToCursor2();
            }
            else
            {
                SwitchToCursor1();
            }
            return;
        }

        // Controleer of het een Binder-tag is en of je papier vasthoudt
        foreach (string tag in binderTags)
        {
            if (obj.CompareTag(tag) && PaperInteraction.isHoldingPaper)
            {
                SwitchToCursor2();
                return;
            }
        }

        // Controleer Plant1 met waterTimer1 <= 10
        if (obj.CompareTag("Plant1"))
        {
            if (WateringSystem.waterTimer1 <= 10)
            {
                SwitchToCursor2();
            }
            else
            {
                SwitchToCursor1();
            }
            return;
        }

        // Controleer Plant2 met waterTimer2 <= 10
        if (obj.CompareTag("Plant2"))
        {
            if (WateringSystem.waterTimer2 <= 10)
            {
                SwitchToCursor2();
            }
            else
            {
                SwitchToCursor1();
            }
            return;
        }

        // Als geen van de bovenstaande condities waar zijn, schakel naar Cursor1
        SwitchToCursor1();
    }

    // Functie voor als de cursor een object verlaat
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

    // Extra functie om de cursorstatus voor het huidige object te controleren
    void CheckCursorStatusForCurrentObject(GameObject obj)
    {
        if (obj.CompareTag("PaperStack"))
        {
            if (!PaperInteraction.isHoldingPaper)
            {
                SwitchToCursor2();
            }
            else
            {
                SwitchToCursor1();
            }
        }
        else if (obj.CompareTag("Plant1"))
        {
            if (WateringSystem.waterTimer1 <= 10)
            {
                SwitchToCursor2();
            }
            else
            {
                SwitchToCursor1();
            }
        }
        else if (obj.CompareTag("Plant2"))
        {
            if (WateringSystem.waterTimer2 <= 10)
            {
                SwitchToCursor2();
            }
            else
            {
                SwitchToCursor1();
            }
        }
        else
        {
            // Controleer of het een Binder-tag is en of je papier vasthoudt
            foreach (string tag in binderTags)
            {
                if (obj.CompareTag(tag) && PaperInteraction.isHoldingPaper)
                {
                    SwitchToCursor2();
                    return;
                }
            }

            // Geen specifieke condities, standaard naar Cursor1
            SwitchToCursor1();
        }
    }
}
