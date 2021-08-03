using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BuildingLight : NetworkBehaviour
{
    [SerializeField] private Light2D[] lights;

    public float minInterval;
    public float maxInterval;
    public float flickerDelay;
    public bool flicker;

    private float flickerTimer;

    // Start is called before the first frame update
    void Start()
    {
        flicker = true;

        if (isServer)
        {
            RpcFlickerLights();
        }
    }

    [ClientRpc]
    private void RpcFlickerLights()
    {
        StartCoroutine(FlickerLights());
    }

    private IEnumerator FlickerLights()
    {
        yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

        while (flicker)
        {
            yield return new WaitForSeconds(flickerDelay);

            int random = Random.Range(0, lights.Length);
            lights[random].enabled = !lights[random].enabled;

            flickerTimer += Time.deltaTime;

            if (flickerTimer >= 0.75f)
            {
                flicker = false;
                flickerTimer = 0f;

                for (int i = 0; i < lights.Length; i++)
                {
                    lights[i].enabled = false;
                }
            }
        }

        if (!flicker)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            flicker = true;
        }

        StartCoroutine(FlickerLights());
    }
}
