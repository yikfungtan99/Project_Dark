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
    KEYBOARD1,
    KEYBOARD2,
    GAMEPAD
}

public class PlayerNetwork : NetworkBehaviour
{
    [SyncVar] public bool localPlayer = false;
    [SyncVar] public ControllerType controller;
    [SyncVar] public int gamepadNum = -1;

    private bool keyboardSplit = false;

    [SyncVar] public string playerName;
    [SerializeField] private GameObject playerPrefab;

    [SyncVar] public int numOfLocalPlayer = 1;
    public bool AddedToLobby = false;

    private NetworkManagerCustom nm;

    private void Start()
    {
        nm = NetworkManagerCustom.Instance;

        if (GameObject.Find("PlayerList"))
        {
            Transform pl = GameObject.Find("PlayerList").transform;
            if (pl)
            {
                transform.SetParent(pl);
                if (hasAuthority)
                {
                    CmdAddNetworkPlayer();
                }
            }
        }
    }

    [Command (ignoreAuthority = true)]
    private void CmdSetPlayerName(string name)
    {
        if (localPlayer)
        {
            playerName = name + numOfLocalPlayer;
        }
        else
        {
            playerName = name;
        }
        
    }

    [Command]
    private void CmdAddNetworkPlayer()
    {
        if (nm)
        {
            if (PlayerPrefs.HasKey("playerName"))
            {
                CmdSetPlayerName(PlayerPrefs.GetString("playerName"));
            }
           
            RpcAddNetworkPlayer();
        }
    }

    [ClientRpc]
    private void RpcAddNetworkPlayer()
    {
        nm.AddNetworkPlayer(this);
    }

    private void Update()
    {
        if (!hasAuthority) return;
        //if (Input.GetKeyDown(KeyCode.K)) SpawnPlayer(netIdentity.connectionToClient);
        RequestLocalPlayer();
    }

    [Command(ignoreAuthority = true)]
    public void CmdSpawnPlayer(int num)
    {
        GameObject player = Instantiate(nm.spawnPrefabs[0]);
        player.GetComponent<PlayerInput>().controller = controller;
        player.GetComponent<PlayerInput>().gamepadNum = num;
        NetworkServer.Spawn(player, connectionToClient);
    }


    private void RequestLocalPlayer()
    {
        if (!Application.isFocused) return;
        if (SceneManager.GetActiveScene().name != "Lobby") return;
        if (!nm) return;
        if (localPlayer) return;
        if (nm.numOfActivePlayers >= 4) return;
        if(numOfLocalPlayer < 4)
        {
            if (Input.GetKeyDown(KeyCode.Slash) && controller == ControllerType.KEYBOARD1 && !keyboardSplit)
            {
                CmdCreateLocalPlayer(ControllerType.KEYBOARD2, -1,netIdentity.connectionToClient);
                keyboardSplit = true;
            }

            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                int cnum = -1;
                print(Gamepad.current.name + "   selected");
                for (int i = 0; i < Gamepad.all.Count; i++)
                {
                    if (Gamepad.current.name == Gamepad.all[i].name)
                    {
                        print(Gamepad.all[i].name + "   match");
                        cnum = i;
                        print(cnum);
                        break;
                    }
                }

                CmdCreateLocalPlayer(ControllerType.GAMEPAD, cnum, netIdentity.connectionToClient);
            }
        }
    }

    [Command]
    private void CmdCreateLocalPlayer(ControllerType con,int cnum,NetworkConnectionToClient conn = null)
    {
        GameObject player = Instantiate(nm.playerPrefab);
        PlayerNetwork p = player.GetComponent<PlayerNetwork>();
        numOfLocalPlayer += 1;

        p.localPlayer = true;
        p.numOfLocalPlayer = numOfLocalPlayer;
        p.gamepadNum = cnum;
        p.controller = con;
        
        NetworkServer.Spawn(player, conn);
    }

    //[ClientRpc]
    //private void RpcCreateLocalPlayer()
    //{
    //    StartCoroutine(WaitForLocalPlayer());
    //}

    //IEnumerator WaitForLocalPlayer()
    //{
    //    yield return new WaitForSeconds(0.02f);
    //    LobbyManager.Instance.AddLobbyPlayer(playerName + "(" + numOfLocalPlayer + ")", true);
    //}

    
}
