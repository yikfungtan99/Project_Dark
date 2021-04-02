using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    [SerializeField] private Transform[] point;
    [SerializeField] private float spd;

    private int currentPoint = 0;

    // Update is called once per frame
    void Update()
    {

        if (transform.position.x != point[currentPoint].position.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, point[currentPoint].position, spd * Time.deltaTime);
        }
        else
        {
            if (currentPoint + 1 > point.Length - 1) { 
                currentPoint = 0;
            }
            else
            {
                currentPoint += 1;
            }
        }
    }
}
