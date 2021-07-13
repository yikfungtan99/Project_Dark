using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Lightning : NetworkBehaviour
{
    public float minInterval;
    public float maxInterval;
    public float minIntensity;
    public float maxIntensity;
    public float minflashInterval;
    public float maxflashInterval;
    public float flashTimer;
    public bool canStrike;
    public bool fadeIntensity;

    [SerializeField] private Light2D lightningFlash;
    [SerializeField] private Animator lightningAnimator;

    // Start is called before the first frame update
    void Start()
    {
        canStrike = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            if (GlobalLight.Instance.strikeThunder)
            {
                if (!canStrike)
                {
                    StartCoroutine(LightningFX());
                    canStrike = true;
                }
            }
        }

        if (fadeIntensity)
            lightningFlash.intensity -= Time.deltaTime;

        if (lightningFlash.intensity <= 0)
            lightningFlash.intensity = 0;
    }

    [ClientRpc]
    private void RpcFlashLighning(float lightningIntensity)
    {
        lightningAnimator.Play("StrikeLightning");
        lightningFlash.gameObject.SetActive(true);
        lightningFlash.intensity = lightningIntensity;
    }

    [ClientRpc]
    private void RpcStrikeThunder(float thunderInterval)
    {
        StartCoroutine(ThunderFX(thunderInterval));
    }

    private IEnumerator LightningFX()
    {
        yield return new WaitForSeconds(Random.Range(minflashInterval, maxflashInterval));

        float li = Random.Range(minIntensity, maxIntensity);
        RpcFlashLighning(li);

        float thunderDelay = Random.Range(0.25f, 1f);
        RpcStrikeThunder(thunderDelay);
    }

    private IEnumerator ThunderFX(float thunderDelay)
    {
        yield return new WaitForSeconds(thunderDelay);
        if (AudioManager.Instance != null) AudioManager.Instance.Play("Lightning");

        yield return new WaitForSeconds(flashTimer);

        fadeIntensity = true;

        yield return new WaitForSeconds(thunderDelay);
        canStrike = false;
        lightningFlash.gameObject.SetActive(false);
    }
}
