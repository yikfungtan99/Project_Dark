using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultPlayerPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtPlayerName;
    [SerializeField] private TextMeshProUGUI txtPlayerWins;

    public void Spawn(int playerNum, int wins)
    {
        txtPlayerName.text = "Player " + playerNum;
        txtPlayerWins.text = wins.ToString();
    }
}
