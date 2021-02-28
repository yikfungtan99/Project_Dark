using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject playerHudPanel;
    [SerializeField] private Transform panels;
    [SerializeField] private TextMeshProUGUI txtWin;

    private PlayerHUDPanel[] playerPanels;
    private PlayerStats[] playerStats;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    public void SpawnPanels()
    {
        playerPanels = new PlayerHUDPanel[gm.transform.childCount];
        playerStats = new PlayerStats[gm.transform.childCount];

        for (int i = 0; i < gm.transform.childCount; i++)
        {
            PlayerHUDPanel p = Instantiate(playerHudPanel, panels).GetComponent<PlayerHUDPanel>();
            playerPanels[i] = p;
        }

        UpdatePlayerHUD();
    }

    public void UpdatePlayerHUD()
    {
        for (int i = 0; i < gm.transform.childCount; i++)
        {
            playerPanels[i].playerStats = gm.transform.GetChild(i).GetComponent<PlayerStats>();
        }
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
        yield return new WaitForSeconds(1);
        txtWin.gameObject.SetActive(false);
    }
}
