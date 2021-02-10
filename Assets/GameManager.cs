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
            PlayerLobby pn = nm.playerList.transform.GetChild(i).GetComponent<PlayerLobby>();
            pn.CmdSpawnPlayer(pn.gamepadNum);
        }
    }
}
