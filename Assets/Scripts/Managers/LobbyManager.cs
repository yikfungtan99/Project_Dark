using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class Level
{
    [Scene] public string levelScene;
    public string levelName;
    public Sprite levelImage;
}

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

    public UnityEvent audioJoin;
    public UnityEvent audioDisconnect;

    [Header("Level Selection")]
    [SerializeField] private Level[] levels;
    [SyncVar(hook = nameof(UpdateLevelUI))] public int levelSelect;

    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private Image levelImage;

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

        ChangeLevel(levelSelect);

        if (isServer)
        {
            UpdateLevelUI(levelSelect, levelSelect);
        }
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        UpdateLevelUI(0, 0);
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

        audioDisconnect.Invoke();
    }

    [Command(ignoreAuthority = true)]
    private void CmdDestroyPlayer(GameObject p)
    {
        NetworkServer.Destroy(p);
        nm.numOfActivePlayers -= 1;
        RpcUpdatePlayerNum();
    }

    [ClientRpc]
    private void RpcUpdatePlayerNum()
    {
        StartCoroutine("WaitToUpdatePlayersNumber");
    }
    
    public LobbyPlayer AddLobbyPlayer(string name, ControllerType type)
    {
        LobbyPlayer lp = Instantiate(lobbyPlayer, lobbyPlayerShelf).GetComponent<LobbyPlayer>();
        lp.controllerType = type;
        lp.Setup(name);

        UpdatePlayersNumber();

        audioJoin.Invoke();

        return lp;
    }

    public void UpdatePlayersNumber()
    {
        for (int i = 0; i < lobbyPlayerShelf.transform.childCount; i++)
        {
            lobbyPlayerShelf.transform.GetChild(i).GetComponent<LobbyPlayer>().UpdatePlayerNum(i);
        }
    }

    IEnumerator WaitToUpdatePlayersNumber()
    {
        yield return new WaitForSeconds(0.02f);
        UpdatePlayersNumber();
    }

    public void UpdateServerId(int oldId, int newId)
    {
        StartCoroutine(WaitForNetwork(newId));
    }

    IEnumerator WaitForNetwork(int id)
    {
        yield return new WaitUntil(() => nm == true);

        if (nm.lrm.Available())
        {
            lobbyId = id;
            txtLobbyId.text = "Lobby ID: " + lobbyId;
        }
        else
        {
            txtLobbyId.text = "Offline Lobby";
        }
    }

    public void LeaveLobby()
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

    public void StartGame()
    {
        nm.ServerChangeScene(nm.gameScene);
    }

    public void ChangeLevel(int dir)
    {
        levelSelect += dir;
    }

    public void UpdateLevelUI(int oldValue, int newValue)
    {
        if (levelSelect < 0)
        {
            levelSelect = levels.Length;
        }

        if (levelSelect >= levels.Length)
        {
            levelSelect = 0;
        }

        Level level = levels[levelSelect];

        levelNameText.text = level.levelName;
        levelImage.sprite = level.levelImage;
        nm.gameScene = level.levelScene;
    }
}
