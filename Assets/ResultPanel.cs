using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private Transform SpawnPanel;
    [SerializeField] private ResultPlayerPanel resultPlayerPanelPrefab;

    public void SpawnScorePanel(int player, int wins)
    {
        ResultPlayerPanel resultPlayerPanelInstance = Instantiate(resultPlayerPanelPrefab, SpawnPanel);
        resultPlayerPanelInstance.Spawn(player, wins);
    }

}
