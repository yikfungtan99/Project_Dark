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
        CheckForBattery();
    }

    private void CheckForBattery()
    {
        if (torch.battery.torchBattery <= 0)
        {
            torch.outOfBattery = true;
            if (torch.torchOn)
            {
                CmdTorch();
            }
        }
        else
        {
            torch.outOfBattery = false;
        }
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
