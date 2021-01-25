using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkSettings : MonoBehaviour
{
    private NetworkIdentity net;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        net = GetComponent<NetworkIdentity>() ? GetComponent<NetworkIdentity>() : null;
        rb = GetComponent<Rigidbody2D>() ? GetComponent<Rigidbody2D>() : null;

        if (net)
        {
            if (!net.hasAuthority)
            {
                rb.isKinematic = true;
            }
        }
    }
}
