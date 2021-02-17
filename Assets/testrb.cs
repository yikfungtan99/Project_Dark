using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testrb : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}
