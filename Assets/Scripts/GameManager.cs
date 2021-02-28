using Mirror;
using System.Collections;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    private NetworkManagerCustom nm;

    [SerializeField] private HUDManager hud;

    [SerializeField] private Transform spawnPoints;

    private Transform[] spawnPoint = new Transform[4];

    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private float gameEndedDelayTime;

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
        if (isClientOnly) return;

        SetSpawnPoints();

        SpawnPlayers();
        hud.SpawnPanels();
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
            pn.CmdSpawnPlayer(i + 1, pn.gamepadNum, pos, rot);
        }
    }

    public void CheckerPlayersRemaining()
    {
        int aliveNum = 0;
        foreach (var item in GetComponentsInChildren<PlayerStats>())
        {
            if (item.alive) aliveNum += 1;
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

    private int GetWinner()
    {
        int winNum = -1;
        foreach (var item in GetComponentsInChildren<PlayerStats>())
        {
            if (item.alive) winNum = item.playerNum;
        }
        return winNum;
    }

    private void RestartGame()
    {
        StartCoroutine(GameEndDelay());
    }

    private void DestroyAllPlayers()
    {
        if (isClientOnly) return;
        for (int i = 0; i < transform.childCount; i++)
        {
            NetworkServer.Destroy(transform.GetChild(i).gameObject);
        }
    }

    private IEnumerator GameEndDelay()
    {
        hud.AnnounceWinner(GetWinner());
        yield return new WaitForSeconds(gameEndedDelayTime);
        DestroyAllPlayers();
        yield return new WaitForSeconds(0.1f);
        SpawnPlayers();
        hud.UpdatePlayerHUD();
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
