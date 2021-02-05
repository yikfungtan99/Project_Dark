using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerNetwork : NetworkBehaviour
{
    public bool localPlayer = false;
    [SyncVar] public string playerName;
    [SerializeField] private GameObject playerPrefab;

    private bool splitKeyboard = false;
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

        DontDestroyOnLoad(gameObject);
    }

    [Command]
    private void CmdSetPlayerName()
    {
        if (PlayerPrefs.HasKey("playerName"))
        {
            playerName = PlayerPrefs.GetString("playerName");
        }
    }

    [Command]
    private void CmdAddNetworkPlayer()
    {
        if (nm)
        {
            CmdSetPlayerName();
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
        CreateLocalPlayer();
    }

    //[Command]
    //private void CmdSpawnPlayer(NetworkConnectionToClient conn = null)
    //{
    //    GameObject player = Instantiate(playerPrefab);

    //    if (numOfLocalPlayer > 1)
    //    {
    //        player.GetComponent<PlayerInput>().splitKeyboard = true;
    //    }

    //    NetworkServer.Spawn(player, conn);
    //    numOfLocalPlayer += 1;
    //}

    //private void SpawnPlayer(NetworkConnectionToClient conn)
    //{
    //    CmdSpawnPlayer(conn);
    //}

    private void CreateLocalPlayer()
    {
        if (!nm) return;
        if (localPlayer) return;
        if (nm.numOfActivePlayers >= 4) return;
        if(numOfLocalPlayer < 4)
        {
            if (Input.GetKeyDown(KeyCode.Slash))
            {
                if (!splitKeyboard)
                {
                    print("HI");
                    CmdCreateLocalPlayer();
                    splitKeyboard = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                CmdCreateLocalPlayer();
            }
        }
    }

    [Command]
    private void CmdCreateLocalPlayer()
    {
        numOfLocalPlayer += 1;
        GameObject player = Instantiate(nm.playerPrefab);
        player.GetComponent<PlayerNetwork>().localPlayer = true;
        NetworkServer.Spawn(player, netIdentity.connectionToClient);
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
