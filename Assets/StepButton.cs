using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum buttonMode
{
    STEP,
    STAY
}

public class StepButton : Trigger
{
    [SerializeField] private buttonMode mode;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(mode == buttonMode.STEP)
        {
            if (collision.transform.parent.CompareTag("Player"))
            {
                FireTrigger();
                print("Player Detected");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (mode == buttonMode.STAY)
        {
            if (collision.transform.parent.CompareTag("Player"))
            {
                FireTrigger();
                print("Player Stayed");
            }
        }
    }
}
