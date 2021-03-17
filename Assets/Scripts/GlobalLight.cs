using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GlobalLight : MonoBehaviour
{
    private static GlobalLight _instance;
    public static GlobalLight Instance {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GlobalLight>();
            }

            return _instance;
        }
    }

    public Light2D globalLite;

    public float targetDarkness;
    
    [SerializeField] private float secondsToDark;

    private float intensityOnStart;
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
            if (globalLite.intensity <= targetDarkness) globalLite.intensity = targetDarkness;
            yield return new WaitWhile(() => globalLite.intensity == targetDarkness);
        }
    }
}
