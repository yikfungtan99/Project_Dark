using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GlobalLight : MonoBehaviour
{

    [SerializeField] private Light2D globalLite;
    [SerializeField] private float intensityOnStart;

    // Start is called before the first frame update
    void Start()
    {
        globalLite.intensity = intensityOnStart;
    }
}
