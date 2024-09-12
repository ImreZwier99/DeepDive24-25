using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnoyingMan : MonoBehaviour
{
    public Transform character;
    [SerializeField] private float currentTime ,unactiveTime = 40, activeTime = 15, time;
    public Animator boss_Animator;
    public static bool isActive = false;
    private int dayCounter;
    // Start is called before the first frame update
    void Start()
    {
        time = unactiveTime;
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        AnnoyingManActive();
        dayCounter = DigitalClock.dayIndex;
        UpdateTimer();
    }

    void AnnoyingManActive()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= time)
        {
            currentTime = 0;
            isActive = !isActive;
            if (isActive)
            {
                boss_Animator.SetBool("BossMovement", true);
                time = activeTime;
            }
            else
            {
                boss_Animator.SetBool("BossMovement", false);
                time = unactiveTime;
            }
        }
    }

    void UpdateTimer()
    {
        if (dayCounter == 2) unactiveTime = 30;
        else if (dayCounter == 3)
        {
            unactiveTime = 25;
            activeTime = 10;
        }
        else if (dayCounter == 4) unactiveTime = 20;
    }
}