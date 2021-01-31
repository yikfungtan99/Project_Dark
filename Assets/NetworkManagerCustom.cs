using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using LightReflectiveMirror;

public class NetworkManagerCustom : NetworkManager
{

    LightReflectiveMirrorTransport lrm;

    private static NetworkManagerCustom _instance;

    public static NetworkManagerCustom Instance { get { return _instance; } }

    public override void Awake()
    {
        base.Awake();
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        try
        {
            lrm = (LightReflectiveMirrorTransport)transport;
        }
        catch (System.Exception)
        {

            print("Not relay transport loaded!");
        }
    }

    public void ConnectToRelay()
    {
        lrm.ConnectToRelay();
    }

    public void DisconnectFromRelay()
    {
        lrm.Shutdown();
    }
}
