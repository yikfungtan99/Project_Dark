using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private void Update()
    {
        if (!hasAuthority) return;
        if (Input.GetKeyDown(KeyCode.K)) SpawnPlayer(netIdentity.connectionToClient);
    }

    [Command]
    private void CmdSpawnPlayer(NetworkConnectionToClient conn = null)
    {
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.Spawn(player, conn);
    }

    private void SpawnPlayer(NetworkConnectionToClient conn)
    {
        CmdSpawnPlayer(conn);
    }
}
