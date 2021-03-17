using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Revealer : MonoBehaviour
{
    [SerializeField] private Light2D torch;
    [SerializeField] private LayerMask torchPlayerLayer;

    [SerializeField] private bool useRaycast;
    [SerializeField] private bool useCollider;

    [SerializeField] private Vector2 circleOffset;
    [SerializeField] private float circleRadius;

    // Update is called once per frame
    void LateUpdate()
    {
        Reveal();
    }

    private void Reveal()
    {
        if (useRaycast)
        {
            if (!torch.enabled || !torch.gameObject.activeSelf) return;
            RaycastHit2D[] hitPlayer = Physics2D.RaycastAll(transform.position, transform.up, torch.pointLightOuterRadius, torchPlayerLayer);

            if (hitPlayer.Length > 0)
            {
                for (int i = 0; i < hitPlayer.Length; i++)
                {
                    if (hitPlayer[i].collider.transform.parent != transform.parent) hitPlayer[i].collider.GetComponentInParent<PlayerStats>().reveal = true;
                }
            }
        }


        if (useCollider)
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll((Vector2)transform.position + circleOffset, circleRadius, torchPlayerLayer);

            if (hit.Length > 0)
            {
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].transform.parent.CompareTag("Player"))
                    {
                        hit[i].transform.GetComponentInParent<PlayerStats>().reveal = true;
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + circleOffset, circleRadius);
    }

}
