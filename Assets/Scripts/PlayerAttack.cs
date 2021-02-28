using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerAttack : NetworkBehaviour
{
    [SerializeField] private Transform attackCircle;
    [SerializeField] private float attackRate;
    private bool canAttack = true;

    [SerializeField] private float attackRadius;
    private Collider2D[] cirCol;
    [SerializeField] private LayerMask attackLayer;

    [SerializeField] private Light2D attackEffects;

    private void Update()
    {
        PlayerDetection();
    }

    private void PlayerDetection()
    {
        cirCol = Physics2D.OverlapCircleAll(attackCircle.position, attackRadius, attackLayer);

        if (cirCol.Length > 0)
        {
            for (int i = 0; i < cirCol.Length; i++)
            {
                if (cirCol[i].transform.parent.gameObject != gameObject)
                {
                    //Another Player within range
                    PlayerStats ph = cirCol[i].gameObject.GetComponentInParent<PlayerStats>();
                    Attack(ph);
                }
            }
        }

        //Remove when better attack effects are here
        if (attackEffects.intensity > 0) attackEffects.intensity = Mathf.Lerp(attackEffects.intensity, 0, 10 * Time.deltaTime);
        if (attackEffects.intensity < 0.01) attackEffects.intensity = 0;
    }

    private void Attack(PlayerStats target)
    {
        if (canAttack)
        {
            target.ModifyHealth(-1);
            attackEffects.intensity = Random.Range(0.8f, 1.0f);
            StartCoroutine("AttackCooldown");
        }
    }

    //private void CmdAttackEffects()
    //{
    //    RpcAttackEffects();
    //}

    //[ClientRpc]
    //private void RpcAttackEffects()
    //{
    //    attackEffects.intensity = Random.Range(0.8f, 1.0f);
    //    StartCoroutine("AttackCooldown");
    //}

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackRate);
        canAttack = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
