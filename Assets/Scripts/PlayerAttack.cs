using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using static UnityEngine.InputSystem.InputAction;

public class PlayerAttack : NetworkBehaviour
{

    private PlayerStats stats;

    [SerializeField] private Transform attackCircle;
    [SerializeField] private float attackRate;
    private bool canAttack = true;

    [SerializeField] private float attackRadius;
    private Collider2D[] cirCol;
    [SerializeField] private LayerMask attackLayer;

    [SerializeField] private Light2D attackEffects;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    public void AttackCall(CallbackContext ctx)
    {
        if (!hasAuthority) return;
        Attack();
    }

    //private void PlayerDetection()
    //{
    //    cirCol = Physics2D.OverlapCircleAll(attackCircle.position, attackRadius, attackLayer);

    //    if (cirCol.Length > 0)
    //    {
    //        for (int i = 0; i < cirCol.Length; i++)
    //        {
    //            if (cirCol[i].transform.parent.gameObject != gameObject)
    //            {
    //                //Another Player within range
    //                PlayerStats ph = cirCol[i].gameObject.GetComponentInParent<PlayerStats>();
    //                Attack(ph);
    //            }
    //        }
    //    }
    //}

    private void Attack()
    {
        PlayerStats target = null;

        if (canAttack && !stats.grace)
        {
            cirCol = Physics2D.OverlapCircleAll(attackCircle.position, attackRadius, attackLayer);

            if (cirCol.Length > 0)
            {
                for (int i = 0; i < cirCol.Length; i++)
                {
                    if (cirCol[i].transform.parent.gameObject != gameObject)
                    {
                        //Another Player within range
                        target = cirCol[i].gameObject.GetComponentInParent<PlayerStats>();
                    }
                }
            }

            if (target)
            {
                target.ModifyHealth(-1);
            }

            attackEffects.intensity = Random.Range(0.5f, 1.0f);
            StartCoroutine("AttackCooldown");
            CmdAttackEffects();
        }
    }

    [Command(ignoreAuthority = true)]
    private void CmdAttackEffects()
    {
        RpcAttackEffects();
    }

    [ClientRpc]
    private void RpcAttackEffects()
    {
        attackEffects.gameObject.SetActive(true);
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackRate);
        canAttack = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCircle.position, attackRadius);
    }
}
