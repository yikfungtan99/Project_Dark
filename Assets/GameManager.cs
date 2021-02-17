using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GameManager : NetworkBehaviour
{
    private NetworkManagerCustom nm;

    [SerializeField] private Transform spawnPoints;

    private Transform[] spawnPoint = new Transform[4];

    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        nm = NetworkManagerCustom.Instance;
        if (isClientOnly) return;

        SetSpawnPoints();

        SpawnPlayers();
    }

    private void Update()
    {
        PauseMenuInput();
    }

    private void PauseMenuInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
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

            if (i == 1 || i == 3) 
            { 
                rot = 180; 
            } 
            else
            {
                rot = 0;
            }

            Vector3 pos = spawnPoint[i].position;
            PlayerLobby pn = nm.playerList.transform.GetChild(i).GetComponent<PlayerLobby>();
            pn.CmdSpawnPlayer(pn.gamepadNum, pos, rot);
        }
    }

    public void Disconnect()
    {
        nm.numOfActivePlayers = 0;

        if (nm.lrm.Available())
        {
            nm.StopServer();
            nm.StopHost();
            nm.StopClient();
            nm.lrm.Shutdown();
        }
        else
        {
            nm.StopServer();
            nm.StopHost();
        }
    }
}
