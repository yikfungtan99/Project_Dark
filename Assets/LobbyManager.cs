using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour
{
    NetworkManagerCustom nm;

    private static LobbyManager _instance;

    public static LobbyManager Instance { get { return _instance; } }

    [SerializeField] private Transform lobbyPlayerShelf;
    [SerializeField] private GameObject lobbyPlayer;

    [SerializeField] private GameObject playerList;

    [SerializeField] private TextMeshProUGUI txtLobbyId;

    public void Awake()
    {
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
    void Start()
    {
        nm = NetworkManagerCustom.Instance ? NetworkManagerCustom.Instance : null;

        playerList = nm.playerList;
    }

    public void AddLobbyPlayer(string name, bool localPlayer)
    {
        LobbyPlayer lp = Instantiate(lobbyPlayer,lobbyPlayerShelf).GetComponent<LobbyPlayer>();
        nm.numOfActivePlayers += 1;
        lp.SetName(name);

    }

    [ClientRpc]
    public void RpcServerId(int id)
    {
        txtLobbyId.text = "Lobby ID: " + id;
    }

    public void UpdateLobby()
    {
        StartCoroutine(WaitForPlayerJoin());
    }

    private IEnumerator WaitForPlayerJoin()
    {
        yield return new WaitForSeconds(0.02f);
        for (int i = 0; i < playerList.transform.childCount; i++)
        {
            PlayerNetwork p = playerList.transform.GetChild(i).GetComponent<PlayerNetwork>();
            if (!p.AddedToLobby)
            {
                AddLobbyPlayer(p.playerName, false);
                p.AddedToLobby = true;
            }
        }
    }

    public void LeaveLobby()
    {
        nm.numOfActivePlayers = 0;
        nm.StopHost();
        nm.StopClient();
        nm.lrm.Shutdown();
    }

    public void StartGame()
    {
        nm.ServerChangeScene(nm.gameScene);
    }
}
