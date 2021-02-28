using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private Image imgController;

    [SerializeField] private Sprite[] imgTypes;

    public ControllerType controllerType;

    public void Setup(string n)
    {
        txtName.text = n;
        imgController.sprite = imgTypes[(int)controllerType - 1];
    }
}
