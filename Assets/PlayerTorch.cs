using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTorch : NetworkBehaviour
{

    [SerializeField] private CustomTorch torch;

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority) return;
        TorchInput();
    }

    void TorchInput()
    {
        if (Input.GetKeyDown(KeyCode.F) && !torch.outOfBattery)
        {
            CmdTorch();
        }
    }

    [Command]
    void CmdTorch()
    {
        RpcTorch();
    }

    [ClientRpc]
    void RpcTorch()
    {
        torch.Torch();
    }
}
