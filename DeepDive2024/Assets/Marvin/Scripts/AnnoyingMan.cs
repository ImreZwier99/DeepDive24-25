using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnoyingMan : MonoBehaviour
{
    public Transform character;
    [SerializeField] private float range, timer;
    public Animator boss_Animator;
    public static bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        range = UnityEngine.Random.Range(20, 61);
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (DigitalClock.dayIndex >= 1 && !isActive) AnnoyingManActive();
    }

    void AnnoyingManActive()
    {
        timer += Time.deltaTime;
        if (timer >= range)
        {
            range = UnityEngine.Random.Range(20, 61);
            boss_Animator.SetBool("BossMovement", true);
            timer = 0;
            isActive = true;
        }
    }
}
