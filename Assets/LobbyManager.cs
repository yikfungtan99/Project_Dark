using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    NetworkManagerCustom nm;
    PlayersManager pm;

    private static LobbyManager _instance;

    public static LobbyManager Instance { get { return _instance; } }

    [SerializeField] private Transform lobbyPlayerShelf;
    [SerializeField] private GameObject lobbyPlayer;

    private int localPlayer;

    [SerializeField] private GameObject playerList;

    [SyncVar(hook = nameof(UpdateServerId))] 
    public int lobbyId;

    [SerializeField] private TextMeshProUGUI txtLobbyId;
    [SerializeField] private Button btnStart;

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
        pm = GetComponent<PlayersManager>();
        playerList = nm.playerList;
        
    }

    private void Update()
    {
        RequestPlayerInput();
        RestrictStartButton();
    }

    public void RemoveLobbyPlayer()
    {
        throw new NotImplementedException();
    }

    void RestrictStartButton()
    {
        btnStart.interactable = isServer && nm.numOfActivePlayers > 0;
    }

    private void RequestPlayerInput()
    {
        if (!Application.isFocused) return;
        if (SceneManager.GetActiveScene().name != "Lobby") return;
        if (!nm) return;
       
        if (Input.GetKeyDown(KeyCode.F))
        {
            print("F");
            if (pm.StorePlayerControlType(ControllerType.KEYBOARD1)){
                CmdRequestPlayer(PlayerPrefs.GetString("playerName"), ControllerType.KEYBOARD1, -1, netIdentity.connectionToClient);
            }
            else
            {
                RemovePlayer(ControllerType.KEYBOARD1, -1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Slash))
        {
            print("/");
            if (pm.StorePlayerControlType(ControllerType.KEYBOARD2))
            {
                CmdRequestPlayer(PlayerPrefs.GetString("playerName"), ControllerType.KEYBOARD2, -1, netIdentity.connectionToClient);
            }
            else
            {
                RemovePlayer(ControllerType.KEYBOARD2, -1);
            }
        }

        if (Gamepad.current != null)
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                print("G");
                int cnum = -1;
                for (int i = 0; i < Gamepad.all.Count; i++)
                {
                    if (Gamepad.current.name == Gamepad.all[i].name)
                    {
                        cnum = i;
                        break;
                    }
                }

                if (pm.StorePlayerControlType(ControllerType.GAMEPAD, cnum))
                {
                    CmdRequestPlayer(PlayerPrefs.GetString("playerName"), ControllerType.GAMEPAD, cnum, netIdentity.connectionToClient);
                }
                else
                {
                    RemovePlayer(ControllerType.GAMEPAD, cnum);
                }
            }
        }
    }

    [Command(ignoreAuthority = true)]
    private void CmdRequestPlayer(string playerName,ControllerType con, int gnum, NetworkConnectionToClient conn = null)
    {
        if (nm.numOfActivePlayers >= 4) return;
        GameObject player = Instantiate(nm.spawnPrefabs[1]);
        PlayerLobby p = player.GetComponent<PlayerLobby>();
        p.playerName = playerName;
        p.gamepadNum = gnum;
        p.controller = con;

        NetworkServer.Spawn(player, conn);
        nm.numOfActivePlayers += 1;
    }

    //Find my player and remove it
    private void RemovePlayer(ControllerType con, int gnum)
    {
        foreach (PlayerLobby pl in playerList.transform.GetComponentsInChildren<PlayerLobby>())
        {
            if (pl.controller == con && pl.gamepadNum == gnum && pl.hasAuthority)
            {
                CmdDestroyPlayer(pl.gameObject);
            }
        }
    }

    [Command(ignoreAuthority = true)]
    private void CmdDestroyPlayer(GameObject p)
    {
        NetworkServer.Destroy(p);
        nm.numOfActivePlayers -= 1;
    }
    
    public LobbyPlayer AddLobbyPlayer(string name, ControllerType type)
    {
        LobbyPlayer lp = Instantiate(lobbyPlayer,lobbyPlayerShelf).GetComponent<LobbyPlayer>();
        lp.controllerType = type;
        lp.Setup(name);

        return lp;
    }

    public void UpdateServerId(int oldId, int newId)
    {
        if(newId != -1)
        {
            lobbyId = newId;
            txtLobbyId.text = "Lobby ID: " + lobbyId;
        }
    }

    public void LeaveLobby()
    {
        nm.numOfActivePlayers = 0;
        
        nm.StopHost();
        nm.StopClient();

        if (nm.lrm.Available()) 
        { 
            nm.lrm.Shutdown();
        }
        SceneManager.LoadScene(nm.offlineScene);
    }

    public void StartGame()
    {
        nm.ServerChangeScene(nm.gameScene);
    }
}
