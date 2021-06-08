﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHUDPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtHealth;
    [SerializeField] private TextMeshProUGUI txtBattery;

    public PlayerStats playerStats;

    // Update is called once per frame
    void Update()
    {
        if (playerStats)
        {
            txtName.text = "Player " + (playerStats.playerNum);
            txtHealth.text = "Health:" + playerStats.health;
            txtBattery.text = "Battery: " + playerStats.battery;
        }
    }
}
