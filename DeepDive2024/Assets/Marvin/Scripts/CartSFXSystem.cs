using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartSFXSystem : MonoBehaviour
{
    public AudioSource cartSFX;
    public AudioSource paperSFX;
    public Animator cart_Animator;
    [SerializeField] private float range, timer;
    public bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        range = Random.Range(120, 161);
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (DigitalClock.dayIndex >= 3 && !isActive) CartLady();
    }

    void CartLady()
    {
        timer += Time.deltaTime;
        if (timer >= range)
        {
            range = Random.Range(120, 161);
            cart_Animator.SetBool("CartMovement", true);
            timer = 0;
            isActive = true;
        }
    }

    public void PlayAudio() => cartSFX.Play();

    public void PauseAudio() => cartSFX.Pause();

    public void UnPauseAudio() => cartSFX.UnPause();

    public void PaperBehaviour()
    {
        if (DigitalClock.dayIndex ==3)
        {
            PaperStack.counter += 20;
            PaperStack.numberOfStacks += 4;
        }
        else if (DigitalClock.dayIndex == 4)
        {
            PaperStack.counter += 30;
            PaperStack.numberOfStacks += 6;
        }
        paperSFX.Play();
        PaperStack.updateStacks = true;
    }

    public void AnimReset()
    {
        cartSFX.Stop();
        cart_Animator.SetBool("CartMovement", false);
    }
}
