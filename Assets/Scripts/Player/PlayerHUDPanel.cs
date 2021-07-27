using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerHUDPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private Transform HPRack;
    [SerializeField] private Transform WinRack;
    [SerializeField] private TextMeshProUGUI txtBattery;

    private bool init = false;
    private int win = 0;

    public PlayerStats playerStats;

    private void GiveWinPoint()
    {
        WinRack.GetChild(win).gameObject.SetActive(true);
        win++;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats)
        {
            if (!init)
            {
                playerStats.OnWin += GiveWinPoint;
                init = true;
            }

            txtName.text = "Player " + (playerStats.playerNum);

            //print(playerStats.health);

            for (int i = 0; i < HPRack.childCount; i++)
            {
                HPRack.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < playerStats.health; i++)
            {
                HPRack.GetChild(i).gameObject.SetActive(true);
            }

            txtBattery.text = "Battery: " + playerStats.battery;
        }
        else
        {
            init = false;
        }
    }

    private void OnDisable()
    {
        playerStats.OnWin -= GiveWinPoint;
    }
}
