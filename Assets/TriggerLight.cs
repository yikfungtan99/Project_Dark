using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerLightType
{
    HOLD,
    TIMER,
    TOGGLE
}

public class TriggerLight : LightBulbs
{
    [SerializeField] private TriggerLightType type;
    [SerializeField] private Trigger trigger;

    private bool alreadyTriggered = false;

    private void Start()
    {
        trigger.OnTriggered += Trigger;
    }

    protected override void Trigger()
    {
        switch (type)
        {
            case TriggerLightType.HOLD:

                if (lightOn)
                {
                    LightsOff();
                }
                else
                {
                    LightsOn();
                }

                break;

            case TriggerLightType.TIMER:
                LightsOn();
                StartCoroutine(Timer());
                break;

            case TriggerLightType.TOGGLE:

                if (lightOn)
                {
                    LightsOff();
                }
                else
                {
                    LightsOn();
                }

                break;

            default:
                break;
        }
    }
}
