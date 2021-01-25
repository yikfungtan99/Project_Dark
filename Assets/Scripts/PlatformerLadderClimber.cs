using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerLadderClimber : MonoBehaviour
{
    [SerializeField] private LayerMask ladderLayer;
    [SerializeField] private Vector2 upperLadderSize;
    [SerializeField] private Vector3 upperLadderOffset;
    [SerializeField] private float offLadderDelay;

    [SerializeField] private Vector3 lowerLadderOffset;
    [SerializeField] private float lowerLadderRadius;

    private PlatformerMovement pm;

    private void Start()
    {
        pm = GetComponent<PlatformerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D ladder = Physics2D.OverlapBox(transform.position + upperLadderOffset, upperLadderSize, 0,ladderLayer);

        RaycastHit2D lowerLadder = Physics2D.Raycast(transform.position + lowerLadderOffset, -transform.up, lowerLadderRadius, ladderLayer);
     
        if (ladder)
        {
            pm.onLadder = true;
        }
        else
        {
            StartCoroutine(OffLadder());
        }

        if (lowerLadder)
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
        Gizmos.DrawWireCube(transform.position + upperLadderOffset, upperLadderSize);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position + lowerLadderOffset, transform.position + lowerLadderOffset + (-transform.up * lowerLadderRadius));
    }
}
