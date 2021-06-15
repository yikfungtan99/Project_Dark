using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightBulbs : MonoBehaviour, ISelectable
{
    [SerializeField] private float timer;
    [SerializeField] private Light2D lightBulb;

    protected bool lightOn = false;

    private void Awake()
    {
        lightBulb.gameObject.SetActive(false);
    }

    public void Select()
    {
        LightsOn();
        StartCoroutine(Timer());
    }

    protected virtual void Trigger()
    {
        LightsOn();
        StartCoroutine(Timer());
    }

    protected void LightsOn()
    {
        lightOn = true;
        lightBulb.gameObject.SetActive(true);
    }

    protected void LightsOff()
    {
        lightOn = false;
        lightBulb.gameObject.SetActive(false);
    }

    protected IEnumerator Timer()
    {
        yield return new WaitForSeconds(timer);
        LightsOff();
    }
}
