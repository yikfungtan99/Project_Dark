using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Users;

public class GameManager : NetworkBehaviour
{
    private NetworkManagerCustom nm;

    [SerializeField] private Transform spawnPoints;

    private Transform[] spawnPoint = new Transform[4];

    private void Start()
    {
        nm = NetworkManagerCustom.Instance;
        if (isClientOnly) return;

        SetSpawnPoints();

        SpawnPlayers();
    }

    private void SetSpawnPoints()
    {
        for (int i = 0; i < spawnPoints.childCount; i++)
        {
            spawnPoint[i] = spawnPoints.GetChild(i);
        }
    }

    private void SpawnPlayers()
    {
        float rot = 0;

        for (int i = 0; i < nm.playerList.transform.childCount; i++)
        {
            
            if (i > 1) rot = 180;

            Vector3 pos = spawnPoint[i].position;
            PlayerLobby pn = nm.playerList.transform.GetChild(i).GetComponent<PlayerLobby>();
            pn.CmdSpawnPlayer(pn.gamepadNum, pos, rot);
        }
    }
}
