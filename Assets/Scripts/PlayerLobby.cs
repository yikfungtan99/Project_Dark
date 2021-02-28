using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem;

public enum ControllerType
{
    NONE = default,
    KEYBOARD1,
    KEYBOARD2,
    GAMEPAD
}

public class PlayerLobby : NetworkBehaviour
{
    [SyncVar] public bool localPlayer = false;
    [SyncVar] public ControllerType controller;
    [SyncVar] public int gamepadNum = -1;

    [SyncVar] public string playerName;
    [SerializeField] private GameObject playerPrefab;

    public bool AddedToLobby = false;

    private NetworkManagerCustom nm;
    private LobbyManager lm;

    public LobbyPlayer lobbyUI;

    public PlayerStats playerStats;

    private void Start()
    {
        nm = NetworkManagerCustom.Instance;
        lm = LobbyManager.Instance;

        if (GameObject.Find("PlayerList"))
        {
            Transform pl = GameObject.Find("PlayerList").transform;
            if (pl)
            {
                transform.SetParent(pl);
            }
        }

        AddToLobby();
    }

    private void AddToLobby()
    {
        if (lm)
        {
            lobbyUI = lm.AddLobbyPlayer(playerName, controller);
        }
    }

    private void OnDestroy()
    {
        if (lobbyUI)
        {
            Destroy(lobbyUI.gameObject);
        }
    }


    [Command(ignoreAuthority = true)]
    public void CmdSpawnPlayer(int pNum, int gNum, Vector3 vec, float rot)
    {
        GameObject player = Instantiate(nm.spawnPrefabs[0], vec, Quaternion.Euler(new Vector3(0, rot, 0)));
        player.GetComponent<PlayerStats>().playerNum = pNum;
        player.GetComponent<PlayerInput>().controller = controller;
        player.GetComponent<PlayerInput>().gamepadNum = gNum;
        NetworkServer.Spawn(player, connectionToClient);
    }
}
