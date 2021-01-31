using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : NetworkBehaviour
{
    public int playerNumberOnDevice;
    public bool splitKeyboard = false;

    public Controls controls;

    public Vector2 movementInput;

    PlatformerMovement move;
    PlayerTorch torch;

    private void Awake()
    {
        controls = new Controls();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        move = GetComponent<PlatformerMovement>();
        torch = GetComponent<PlayerTorch>();

        if (hasAuthority)
        {
            playerNumberOnDevice = PlayerManager.Instance.curPlayerNum;
            PlayerManager.Instance.AddPlayerNum();
        }
        else
        {
            print("I don't have authority here");
        }

        if (!splitKeyboard)
        {
            controls.Player1.Enable();
            controls.Player1.Torch.performed += torch.Torch;
            controls.Player1.Jump.performed += move.JumpCall;
        }
        else
        {
            controls.Player2.Enable();
            controls.Player2.Torch.performed += torch.Torch;
            controls.Player2.Jump.performed += move.JumpCall;
        }
    }

    private void OnDisable()
    {
        if (!splitKeyboard)
        {
            controls.Player1.Disable();
            controls.Player1.Torch.performed -= GetComponent<PlayerTorch>().Torch;
            controls.Player1.Jump.performed -= move.JumpCall;
        }
        else
        {
            controls.Player2.Disable();
            controls.Player2.Torch.performed -= GetComponent<PlayerTorch>().Torch;
            controls.Player1.Jump.performed -= move.JumpCall;
        }
    }

    private void Update()
    {
        ReadInput();
    }

    private void ReadInput()
    {
      
        if (!splitKeyboard)
        {
            movementInput = controls.Player1.Movement.ReadValue<Vector2>();
        }
        else
        {
            movementInput = controls.Player2.Movement.ReadValue<Vector2>();
        }
        
    }
}
