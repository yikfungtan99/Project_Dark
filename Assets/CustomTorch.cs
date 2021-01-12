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

    private float radius;

    private PlatformerPlayerSprite rot;

    // Start is called before the first frame update
    void Start()
    {
        rot = GetComponentInParent<PlatformerPlayerSprite>();
    }

    // Update is called once per frame
    void Update()
    {
        Torch();
        RotateTorch();
    }

    private void Torch()
    {
        if (Input.GetKey(KeyCode.F))
        {
            TorchRange();
            selfLight.SetActive(true);
            torch.enabled = true;
        }
        else
        {
            selfLight.SetActive(false);
            torch.enabled = false;
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
