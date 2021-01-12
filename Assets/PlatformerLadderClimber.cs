using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerLadderClimber : MonoBehaviour
{
    [SerializeField] private LayerMask ladderLayer;
    [SerializeField] private float upperLadderRadius;
    [SerializeField] private Vector3 upperLadderOffset;
    [SerializeField] private float offLadderDelay;

    [SerializeField] private float lowerLadderRadius;
    [SerializeField] private Vector3 lowerLadderOffset;

    private PlatformerMovement pm;

    private void Start()
    {
        pm = GetComponent<PlatformerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D ladder = Physics2D.OverlapCircle(transform.position + upperLadderOffset, upperLadderRadius, ladderLayer);
        Collider2D lowerLadder = Physics2D.OverlapCircle(transform.position + lowerLadderOffset, lowerLadderRadius, ladderLayer);

        if (ladder)
        {
            pm.onLadder = true;
        }
        else
        {
            StartCoroutine(OffLadder());
        }

        if (lowerLadder && !ladder)
        {
            pm.ladderBelow = true;
        }
        else
        {
            pm.ladderBelow = false;
        }
    }

    IEnumerator OffLadder()
    {
        yield return new WaitForSeconds(offLadderDelay);
        pm.onLadder = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + upperLadderOffset, upperLadderRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + lowerLadderOffset, lowerLadderRadius);
    }
}
