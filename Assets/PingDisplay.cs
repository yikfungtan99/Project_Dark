using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PingDisplay : NetworkBehaviour
{
    private NetworkManagerCustom net;
    private double ping;
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        net = NetworkManagerCustom.Instance;
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        ping = NetworkTime.rtt / 2;
        text.text = ping.ToString() + " ms";
    }
}
