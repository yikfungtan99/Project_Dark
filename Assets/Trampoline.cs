using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{

    [SerializeField] private float force;
    [SerializeField] private float forceXamp = 3.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {   
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlatformerMovement>().AddExternalForce(CalculateForceAngle());
        }
    }

    private Vector2 CalculateForceAngle()
    {
        Vector2 currentForce = transform.right * force;
        currentForce.x = currentForce.x * forceXamp;
        return currentForce;
    }
}
