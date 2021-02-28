using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHUDPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtHealth;

    public PlayerStats playerStats;

    private void Start()
    {
        txtName.text = "Player " + (playerStats.playerNum);
    }

    // Update is called once per frame
    void Update()
    {
        txtHealth.text = "Health:" + playerStats.health;
    }
}
