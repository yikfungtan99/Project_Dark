﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRevealer : Revealer
{
    public float torchRevealRange = 2;
    private Vector2 initCircleOffset;

    private void Start()
    {
        initCircleOffset = circleOffset;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Detection()
    {
        RaycastHit2D[] players = Physics2D.RaycastAll(transform.parent.position, transform.parent.right * torchRevealRange, torchPlayerLayer);

        if(players.Length > 0)
        {
            for (int i = 0; i < players.Length; i++)
            {
                //print(players[i].collider.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawRay(transform.parent.position, transform.parent.right * torchRevealRange);
    }
}
