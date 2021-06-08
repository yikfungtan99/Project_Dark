using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRevealer : Revealer
{
    private Vector2 initCircleOffset;

    private void Start()
    {
        initCircleOffset = circleOffset;
    }

    protected override void Update()
    {
        RotateCircle();
        base.Update();
    }

    private void RotateCircle()
    {
        float y = transform.parent.rotation.y;
        if (Mathf.Abs(transform.parent.rotation.y) > 0.1)
        {
            circleOffset = initCircleOffset * -1;
        }
        else
        {
            circleOffset = initCircleOffset;
        }
    }
}
