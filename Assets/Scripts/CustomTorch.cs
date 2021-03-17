using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CustomTorch : MonoBehaviour
{
    [SerializeField] private GameObject selfLight;
    [SerializeField] private Light2D torch;
    [SerializeField] private float torchRange;

    [Range(0.1f, 3.0f)]
    [SerializeField] private float torchRangeOffset;
    [SerializeField] private LayerMask torchBlockLayer;
    [SerializeField] private LayerMask torchPlayerLayer;

    public bool torchOn;
    private float radius;

    public TorchBattery battery;
    public bool outOfBattery = false;

    [SerializeField] private PlatformerMovement move;

    // Start is called before the first frame update
    void Start()
    {
        battery = GetComponent<TorchBattery>() ? GetComponent<TorchBattery>() : null;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (torchOn)
        {
            TorchRange();
        }
    }

    public void Torch()
    {
        if (torchOn) {

            TorchOff();

        }
        else
        {
            TorchOn();
        }
    }

    public void TorchOn()
    {
        selfLight.SetActive(true);
        torch.enabled = true;
        torchOn = true;
    }

    public void TorchOff()
    {
        selfLight.SetActive(false);
        torch.enabled = false;
        torchOn = false;
    }

    private void TorchRange()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, torchRange, torchBlockLayer);
        //RaycastHit2D[] hitPlayer = Physics2D.RaycastAll(transform.position, transform.up, torchRange, torchPlayerLayer);

        if (hit)
        {
            torch.pointLightOuterRadius = hit.distance + torchRangeOffset;
        }
        else
        {
            torch.pointLightOuterRadius = torchRange;
        }

       
        //if (hitPlayer.Length > 0)
        //{
        //    for (int i = 0; i < hitPlayer.Length; i++)
        //    {
        //        if (hitPlayer[i].collider.transform.parent != transform.parent) hitPlayer[i].collider.GetComponentInParent<PlayerStats>().reveal = true;
        //    }
        //}
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * torchRange);
    }
}
