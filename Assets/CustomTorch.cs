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

    public bool torchOn;
    private float radius;

    private PlatformerPlayerSprite rot;

    private TorchBattery battery;
    private bool outOfBattery = false;

    // Start is called before the first frame update
    void Start()
    {
        rot = GetComponentInParent<PlatformerPlayerSprite>();
        battery = GetComponent<TorchBattery>() ? GetComponent<TorchBattery>() : null;
    }

    // Update is called once per frame
    void Update()
    {
        if (battery)
        {
            CheckForBattery();
        }
        Torch();
        RotateTorch();
    }

    private void CheckForBattery()
    {
        if(battery.torchBattery <= 0)
        {
            outOfBattery = true;
        }
        else
        {
            outOfBattery = false;
        }
    }

    private void Torch()
    {
        if (Input.GetKey(KeyCode.F) && !outOfBattery)
        {
            TorchRange();
            selfLight.SetActive(true);
            torch.enabled = true;
            torchOn = true;
        }
        else
        {
            selfLight.SetActive(false);
            torch.enabled = false;
            torchOn = false;
        }
    }

    private void RotateTorch()
    {
        if (!rot) return;
        float angle = rot.currentFacingDirection == FacingDirection.LEFT ? 90 : -90;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void TorchRange()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, torchRange, torchBlockLayer);

        if (hit)
        {
            torch.pointLightOuterRadius = hit.distance + torchRangeOffset;
        }
        else
        {
            torch.pointLightOuterRadius = torchRange;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * torchRange);
    }
}
