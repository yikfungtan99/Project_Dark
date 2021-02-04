using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerTorch : NetworkBehaviour
{
    [SerializeField] private CustomTorch torch;


    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority) return;
        //CheckForBattery();
    }

    private void CheckForBattery()
    {
        if (torch.battery.torchBattery <= 0)
        {
            torch.outOfBattery = true;
            if (torch.torchOn)
            {
                CmdTorchOff();
            }
        }
        else
        {
            torch.outOfBattery = false;
        }
    }

    public void Torch(CallbackContext ctx)
    {
        if (!torch.outOfBattery)
        {
            if (!torch.torchOn)
            {
                CmdTorchOn();
            }
            else
            {
                CmdTorchOff();
            }
        }
    }

    [Command]
    public void CmdTorchOn()
    {
        RpcTorchOn();
    }

    [ClientRpc]
    public void RpcTorchOn()
    {
        torch.TorchOn();
    }
    
    [Command]
    public void CmdTorchOff()
    {
        RpcTorchOff();
    }

    [ClientRpc]
    public void RpcTorchOff()
    {
        torch.TorchOff();
    }
}
