using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLight : LightBulbs
{
    [SerializeField] private Trigger trigger;

    private void Start()
    {
        trigger.OnTriggered += Trigger;
    }
}
