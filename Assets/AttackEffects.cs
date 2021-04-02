using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class AttackEffects : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.5f;
    [SerializeField] private Light2D lite;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine("Decay");
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator Decay()
    {
        lite.intensity = Random.Range(0.1f, 0.3f);
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
}
