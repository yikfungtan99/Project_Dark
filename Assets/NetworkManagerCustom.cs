using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using LightReflectiveMirror;
using UnityEngine.SceneManagement;
using System;

public class NetworkManagerCustom : NetworkManager
{
    //[SerializeField] TelepathyTransport tp;
    public LightReflectiveMirrorTransport lrm;

    private static NetworkManagerCustom _instance;

    public static NetworkManagerCustom Instance { get { return _instance; } }

    [Scene] public string lobbyScene;

    public int numOfActivePlayers;
    public List<PlayerNetwork> playerList;

    public override void Awake()
    {
        base.Awake();
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        lrm = (LightReflectiveMirrorTransport)transport;
        playerList = new List<PlayerNetwork>();
    }

    public void ConnectToRelay()
    {
        lrm.ConnectToRelay();
    }

    public void CreateLobby()
    {
        StartHost();
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        print("Hosted Server");
        ServerChangeScene(lobbyScene);
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
    }

    public void AddNetworkPlayer(PlayerNetwork pn)
    {
        playerList.Clear();

        GameObject pl = GameObject.Find("PlayerList");

        for (int i = 0; i < pl.transform.childCount; i++)
        {
            playerList.Add(pl.transform.GetChild(i).GetComponent<PlayerNetwork>());
        }

        LobbyManager.Instance.UpdateLobby();
    }

}
