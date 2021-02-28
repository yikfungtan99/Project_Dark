using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GlobalLight : MonoBehaviour
{
    [SerializeField] private Light2D globalLite;
    [SerializeField] private float intensityOnStart;
    [SerializeField] private float secondsToDark;
    float curIntensity = 0;

    // Start is called before the first frame update
    void Start()
    {
        globalLite.intensity = intensityOnStart;
        StartCoroutine(lightDimmer());
    }

    IEnumerator lightDimmer()
    {
        while (true)
        {
            curIntensity += Time.deltaTime;

            globalLite.intensity = 1.0f - (curIntensity/secondsToDark);
            //curIntensity = Mathf.Clamp01(curIntensity);
            if (globalLite.intensity < 0.1) globalLite.intensity = 0;
            yield return new WaitWhile(() => globalLite.intensity == 0.0);
        }
    }
}
