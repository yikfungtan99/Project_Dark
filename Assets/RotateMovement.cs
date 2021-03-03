using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMovement : MonoBehaviour
{

    [SerializeField] private float rotateSpeed;
    [SerializeField] private float angle = 30.0f;

    [SerializeField] private bool reverse = false;
    private int rev = 1;

    private float rot = 0;
    private float targetAngle;

    // Start is called before the first frame update
    void Start()
    {
        //transform.localEulerAngles = new Vector3(0, 0, -angle/2);
        if (reverse) rev = -1;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!reverse)
        //{
        //    rot = rotateSpeed * Time.deltaTime;
        //    targetAngle = angle;
        //}
        //else
        //{
        //    rot = -rotateSpeed * Time.deltaTime;
        //    targetAngle = -angle;
        //}

        //transform.Rotate(Vector3.forward * rot);
        transform.localEulerAngles = new Vector3(0, 0, Mathf.PingPong(Time.time * rotateSpeed, angle) * rev);
    }
}
