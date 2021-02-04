using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtName;

    public void SetName(string n)
    {
        txtName.text = n;
    }
}
