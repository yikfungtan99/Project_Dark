using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightBulbs : MonoBehaviour, ISelectable
{
    [SerializeField] private float timer;
    [SerializeField] private Light2D lightBulb;

    private void Awake()
    {
        lightBulb.gameObject.SetActive(false);
    }

    public virtual void Trigger()
    {
        LightsOn();
        StartCoroutine(Timer());
    }

    void LightsOn()
    {
        lightBulb.gameObject.SetActive(true);
    }

    void LightsOff()
    {
        lightBulb.gameObject.SetActive(false);
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(timer);
        LightsOff();
    }
}
