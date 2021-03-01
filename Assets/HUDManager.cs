using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject playerHudPanel;
    [SerializeField] private Transform panels;
    [SerializeField] private TextMeshProUGUI txtWin;

    private List<PlayerHUDPanel> playerPanels = new List<PlayerHUDPanel>();

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPanels();
        gm = GameManager.Instance;
    }

    public void SpawnPanels()
    {
        for (int i = 0; i < NetworkManagerCustom.Instance.playerList.transform.childCount; i++)
        {
            playerPanels.Add(Instantiate(playerHudPanel, panels).GetComponent<PlayerHUDPanel>());
        }
    }

    public void UpdatePlayerHUD(PlayerStats stats)
    {
        playerPanels[stats.playerNum - 1].playerStats = stats;
    }

    public void AnnounceWinner(int num)
    {
        txtWin.gameObject.SetActive(true);
        if(num != -1)
        {
            txtWin.text = "PLAYER " + num + " WINS!";
        }
        else
        {
            txtWin.text = "NOBODY " + num + " WINS!";
        }
        StartCoroutine(HideWinText());
    }

    private IEnumerator HideWinText()
    {
        
        yield return new WaitForSeconds(gm.gameEndedDelayTime/2);
        print("Hi");
        txtWin.text = "ROUND " + gm.round + "!";
        yield return new WaitForSeconds(gm.gameEndedDelayTime /2);
        txtWin.gameObject.SetActive(false);
    }
}
