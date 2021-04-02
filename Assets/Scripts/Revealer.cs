using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Revealer : MonoBehaviour
{
    [SerializeField] private Light2D source;
    [SerializeField] private LayerMask torchPlayerLayer;

    [SerializeField] protected Vector2 circleOffset;
    [SerializeField] private float circleRadius;

    List<PlayerStats> detectedPlayer = new List<PlayerStats>();

    // Update is called once per frame
    protected virtual void Update()
    {
        Detection();
    }

    private void Reveal(PlayerStats player)
    {
        player.reveal = true;
    }

    protected virtual void Detection()
    {
        if (source.enabled && source.gameObject.activeSelf)
        {

            Collider2D[] hit = Physics2D.OverlapCircleAll((Vector2)transform.position + circleOffset, circleRadius, torchPlayerLayer);

            if (hit.Length > 0)
            {
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].transform.parent.CompareTag("Player"))
                    {
                        Reveal(hit[i].transform.GetComponentInParent<PlayerStats>());
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
