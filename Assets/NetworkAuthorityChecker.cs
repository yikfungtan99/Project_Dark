using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkAuthorityChecker : MonoBehaviour
{

    [SerializeField] private List<MonoBehaviour> networkedComponents;

    private NetworkIdentity net;

    // Start is called before the first frame update
    void Start()
    {
        net = GetComponent<NetworkIdentity>() ? GetComponent<NetworkIdentity>() : null;

        if (net)
        {
            if (!net.hasAuthority)
            {
                foreach (MonoBehaviour mb in networkedComponents)
                {
                    if(mb) mb.enabled = false;
                }
            }
        }
    }
}
