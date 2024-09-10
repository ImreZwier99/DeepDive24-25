using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothingPanel : MonoBehaviour
{
    public bool smooth;
    public static bool smoothing;

    void Update()
    {
        // Update de statische bool met de waarde van de public bool smooth
        smoothing = smooth;

        // Debugging om te controleren of smoothing wordt bijgewerkt
        Debug.Log("SmoothingPanel: Smoothing set to " + smoothing);
    }
}
