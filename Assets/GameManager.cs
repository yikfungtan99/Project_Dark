using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Users;

public class GameManager : NetworkBehaviour
{
    private NetworkManagerCustom nm;

    private void Start()
    {
        nm = NetworkManagerCustom.Instance;
        if (isClientOnly) return;
        for (int i = 0; i < nm.playerList.transform.childCount; i++)
        {
            PlayerNetwork pn = nm.playerList.transform.GetChild(i).GetComponent<PlayerNetwork>();
            pn.CmdSpawnPlayer(pn.gamepadNum);
        }
    }
}
