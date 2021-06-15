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
        FireTrigger();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.parent.CompareTag("Player"))
        {
            if (mode == buttonMode.STAY)
            {
                FireTrigger();
            }
        }
    }
}
