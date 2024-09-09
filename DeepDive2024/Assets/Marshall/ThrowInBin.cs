using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThrowInBin : MonoBehaviour
{
    public bool lookingToBin = false;
    System.Random rnd = new System.Random();
    public int random;

    void Start()
    {
        
    }

    void Update()
    {
        if (lookingToBin && Input.GetKeyDown(KeyCode.Mouse0))
        {
            random = rnd.Next(0,3);
        }
        if (random <= 1)
        {
            Debug.Log("Gemist");
            random = 5;
        }
        else if (random == 2)
        {
            Debug.Log("Geraakt");
            random = 5;
        }
    }
}
