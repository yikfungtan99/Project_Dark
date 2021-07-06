using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Lightning : MonoBehaviour
{
    public float minInterval;
    public float maxInterval;
    public float minIntensity;
    public float maxIntensity;
    public float flashInterval;
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
        if (GlobalLight.Instance.strikeThunder)
        {
            if (!canStrike)
            {
                StartCoroutine(LightningFX());
                canStrike = true;
            }
        }

        if (fadeIntensity)
            lightningFlash.intensity -= Time.deltaTime;

        if (lightningFlash.intensity <= 0)
            lightningFlash.intensity = 0;
    }

    private IEnumerator LightningFX()
    {
        yield return new WaitForSeconds(flashInterval);

        lightningAnimator.Play("StrikeLightning");
        lightningFlash.intensity = Random.Range(minIntensity, maxIntensity);

        StartCoroutine(ThunderFX());

        yield return new WaitForSeconds(flashTimer);

        fadeIntensity = true;

        yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
        canStrike = false;
    }

    private IEnumerator ThunderFX()
    {
        yield return new WaitForSeconds(Random.Range(0.25f, 1f));

        if (AudioManager.Instance != null) AudioManager.Instance.Play("Lightning");
    }
}
