using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputController
{
    KEYBOARD,
    GAMEPAD
}

public class PlayerInput : MonoBehaviour
{
    public int playerNumberOnDevice;
    public InputController controller;
}
