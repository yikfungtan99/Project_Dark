using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    private NetworkManagerCustom nm;

    public HUDManager hud;

    public List<GameObject> avatarList;

    [SerializeField] private Transform spawnPoints;

    private Transform[] spawnPoint = new Transform[4];

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject helpPanel;

    public float gameEndedDelayTime;

    public Dictionary<int, int> winners = new Dictionary<int, int>();

    [SerializeField] private ResultPanel resultPanel;

    [SyncVar] public int round = 1;

    [SerializeField] private int firstToWinCount = 0;

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

    private void Start()
    {
        nm = NetworkManagerCustom.Instance;
        avatarList = new List<GameObject>();
        if (isClientOnly) return;

        SetSpawnPoints();

        SpawnPlayers();
    }

    private void Update()
    {
        PauseMenuInput();
    }

    public void AddToAvatarList(GameObject avatar)
    {
        avatarList.Add(avatar);
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
        if (isClientOnly) return;
        
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
            pn.CmdSpawnPlayer(i + 1, pn.gamepadNum, pos, rot);

            if(!winners.ContainsKey(i + 1))
            {
                winners.Add(i + 1, 0);
            }
        }
    }

    public void CheckForPlayerRemaining()
    {
        int aliveNum = 0;
        foreach (var item in avatarList)
        {
            PlayerStats s = item.GetComponent<PlayerStats>();
            if (s.alive) aliveNum += 1;     
        }

        if(aliveNum <= 1)
        {
            if (aliveNum == 0)
            {
                print("GameEnded in tie");
            }
            else if (aliveNum == 1)
            {
                print("GameEnded");
            }

            RestartGame();
        }
    }

    private PlayerStats GetWinnerPlayer()
    {
        PlayerStats winner = null;

        foreach (var item in avatarList)
        {
            PlayerStats s = item.GetComponent<PlayerStats>();

            if (s.alive)
            {
                winner = s;
            }
        }
        return winner;
    }

    private int GetWinner()
    {
        int winNum = -1;

        foreach (var item in avatarList)
        {
            PlayerStats s = item.GetComponent<PlayerStats>();
            
            if (s.alive) 
            { 
                winNum = s.playerNum;
            }
        }
        return winNum;
    }

    private void RestartGame()
    {
        if (isServer) round += 1;
        GetWinnerPlayer().Win();
        StartCoroutine(GameEndDelay());
    }

    private IEnumerator GameEndDelay()
    {
        bool haveWinner = false;
        hud.AnnounceWinner(GetWinner());

        if (winners.ContainsKey(GetWinner()))
        {
            winners[GetWinner()] += 1;
        }

        yield return new WaitForSeconds(gameEndedDelayTime);

        for (int i = 0; i < winners.Count; i++)
        {
            if(winners[i + 1] >= firstToWinCount)
            {
                print("WE HAVE A WINNER");
                haveWinner = true;
                
                break;
            }
        }

        if (haveWinner)
        {
            ScorePanel();
        }
        else
        {
            DestroyAllPlayers();
            SpawnPlayers();
        }
        //hud.UpdatePlayerHUD();
    }

    private void DestroyAllPlayers()
    {
        if (!isClientOnly)
        {
            for (int i = 0; i < avatarList.Count; i++)
            {
                NetworkServer.Destroy(avatarList[i].gameObject);
            }

            RpcResetAvatarList();
        }
    }

    private void ResetAvatarList()
    {
        avatarList.Clear();
    }

    [ClientRpc]
    private void RpcResetAvatarList()
    {
        ResetAvatarList();
    }

    [ClientRpc]
    private void RpcShowScorePanel()
    {
        resultPanel.gameObject.SetActive(true);
    }

    private void ScorePanel()
    {
        RpcShowScorePanel();
        var sortedWinners = from entry in winners orderby entry.Value descending select entry;
        foreach (KeyValuePair<int, int> win in sortedWinners)
        {
            RpcSpawnScorePanel(win.Key, win.Value);
        }
    }

    [ClientRpc]
    private void RpcSpawnScorePanel(int key, int value)
    {
        resultPanel.SpawnScorePanel(key, value);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
    }

    public void LoadHelpPanel()
    {
        pausePanel.SetActive(false);
        helpPanel.SetActive(true);
    }

    public void LoadPausePanel()
    {
        helpPanel.SetActive(false);
        pausePanel.SetActive(true);
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
