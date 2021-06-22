using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private float moveSpeed;

    [SerializeField] private Transform[] wayPoint;

    private int curWaypoint = 0;

    // Update is called once per frame
    void Update()
    {
        platform.position = Vector2.MoveTowards(platform.position, wayPoint[curWaypoint].position, moveSpeed * Time.deltaTime);

        if (platform.position == wayPoint[curWaypoint].position)
        {
            curWaypoint += 1;

            if (curWaypoint >= wayPoint.Length)
            {
                curWaypoint = 0;
            }
        }
    }
}
