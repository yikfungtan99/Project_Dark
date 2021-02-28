using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireflies : MonoBehaviour
{
    private Vector3 destinationPoint;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        RefreshDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != destinationPoint)
        {
            transform.position = Vector2.MoveTowards(transform.position, destinationPoint, Time.deltaTime);
        }
        else
        {
            RefreshDestination();
        }
    }

    void RefreshDestination()
    {
        float randX = Random.Range(cam.ScreenToWorldPoint(new Vector2(0, 0)).x, (cam.ScreenToWorldPoint(new Vector2(Screen.width/2, 0)).x));
        float randY = Random.Range(cam.ScreenToWorldPoint(new Vector2(0, 0)).y, (cam.ScreenToWorldPoint(new Vector2(0, Screen.height/2)).y));
        destinationPoint = cam.WorldToScreenPoint(new Vector3(randX, randY, cam.nearClipPlane));
    }
}
