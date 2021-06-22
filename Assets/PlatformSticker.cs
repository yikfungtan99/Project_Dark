using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlatformSticker : NetworkBehaviour 
{
    [SerializeField] private Transform stickTransform;

    ConstraintSource src;

    private void Start()
    {
        src = new ConstraintSource();

        src.sourceTransform = stickTransform;
        src.weight = 1;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            GameObject player = collision.transform.gameObject;

            print(player);

            PositionConstraint positionConstraint = player.GetComponent<PositionConstraint>();

            if (positionConstraint != null)
            {
                positionConstraint.AddSource(src);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision != null)
        //{
        //    if (collision.transform.parent.CompareTag("Player"))
        //    {
        //        GameObject player = collision.transform.parent.gameObject;

        //        PositionConstraint positionConstraint = player.GetComponent<PositionConstraint>();

        //        if (positionConstraint != null)
        //        {
        //            for (int i = 0; i < positionConstraint.sourceCount; i++)
        //            {
        //                if (positionConstraint.GetSource(i).sourceTransform == transform)
        //                {
        //                    positionConstraint.RemoveSource(i);
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
