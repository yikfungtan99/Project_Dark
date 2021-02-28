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
    [Scene] public string gameScene;

    public GameObject playerList;
    public int numOfActivePlayers;

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

        lrm = (LightReflectiveMirrorTransport)transport;

    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
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

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        if(sceneName == lobbyScene)
        {
            LobbyManager.Instance.lobbyId = lrm.serverId;
        }
        else if(sceneName == offlineScene)
        {
            for (int i = 0; i < playerList.transform.childCount; i++)
            {
                Destroy(playerList.transform.GetChild(i).gameObject);
            }
        }
        
    }

    //public void AddNetworkPlayer()
    //{
    //    if (LobbyManager.Instance) LobbyManager.Instance.UpdateLobby();
    //}

}
