using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputController
{
    KEYBOARD,
    GAMEPAD
}

public class PlayerInput : NetworkBehaviour
{
    public int playerNumberOnDevice;
    public InputController controller;

    public Controls controls;

    public Vector2 movementInput;

    private void Awake()
    {
        controls = new Controls();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (hasAuthority)
        {
            playerNumberOnDevice = PlayerManager.Instance.curPlayerNum;
            PlayerManager.Instance.AddPlayerNum();
        }
        else
        {
            print("I don't have authority here");
        }
        

        if (controller == InputController.KEYBOARD)
        {
            if (playerNumberOnDevice == 0)
            {
                controls.Player1.Enable();
                controls.Player1.Torch.performed += GetComponent<PlayerTorch>().Torch;
            }
            else
            {
                controls.Player2.Enable();
                controls.Player2.Torch.performed += GetComponent<PlayerTorch>().Torch;
            }
        }
    }

    private void OnDisable()
    {
        if (controller == InputController.KEYBOARD)
        {
            if (playerNumberOnDevice == 0)
            {
                controls.Player1.Disable();
                controls.Player1.Torch.performed -= GetComponent<PlayerTorch>().Torch;
            }
            else
            {
                controls.Player2.Disable();
                controls.Player2.Torch.performed -= GetComponent<PlayerTorch>().Torch;
            }
        }
    }

    private void Update()
    {
        ReadInput();
    }

    private void ReadInput()
    {
        if (controller == InputController.KEYBOARD)
        {
            if (playerNumberOnDevice == 0)
            {
                movementInput = controls.Player1.Movement.ReadValue<Vector2>();
            }
            else
            {
                movementInput = controls.Player2.Movement.ReadValue<Vector2>();
            }
        }
    }
}
