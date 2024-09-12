using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinSFX : MonoBehaviour
{
    public AudioSource binSFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("PaperBall")) binSFX.Play();
    }
}
