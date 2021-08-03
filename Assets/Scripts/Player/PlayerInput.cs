using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerInput : NetworkBehaviour
{
    [SyncVar] public ControllerType controller;

    [SyncVar] public int gamepadNum = 0;
    public int playerNumberOnDevice;

    public Controls controls;

    public Vector2 movementInput;

    PlatformerMovement move;
    PlayerAttack attack;
    PlayerTorch torch;
    [SerializeField] Lever[] lever;

    private void Awake()
    {
        //if (!hasAuthority) return;
        controls = new Controls();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        //if (!hasAuthority) return;

        move = GetComponent<PlatformerMovement>();
        attack = GetComponent<PlayerAttack>();
        torch = GetComponent<PlayerTorch>();
        lever = FindObjectsOfType<Lever>();

        switch (controller)
        {
            case ControllerType.KEYBOARD1:
                controls.Player1.Enable();
                controls.Player1.Torch.performed += torch.Torch;
                controls.Player1.Jump.performed += move.JumpCall;
                controls.Player1.Attack.performed += attack.AttackCall;

                foreach (Lever l in lever)
                {
                    controls.Player1.Lever.performed += l.LeverCall;
                }

                InputUser user1 = InputUser.PerformPairingWithDevice(Keyboard.current);
                user1.AssociateActionsWithUser(controls);
                break;
            case ControllerType.KEYBOARD2:
                controls.Player2.Enable();
                controls.Player2.Torch.performed += torch.Torch;
                controls.Player2.Jump.performed += move.JumpCall;
                controls.Player2.Attack.performed += attack.AttackCall;

                foreach (Lever l in lever)
                {
                    controls.Player2.Lever.performed += l.LeverCall;
                }

                InputUser user2 = InputUser.PerformPairingWithDevice(Keyboard.current);
                user2.AssociateActionsWithUser(controls);
                break;
            case ControllerType.GAMEPAD:
                controls.Player1.Enable();
                controls.Player1.Torch.performed += torch.Torch;
                controls.Player1.Jump.performed += move.JumpCall;
                controls.Player1.Attack.performed += attack.AttackCall;

                foreach (Lever l in lever)
                {
                    controls.Player1.Lever.performed += l.LeverCall;
                }

                InputUser user3 = InputUser.PerformPairingWithDevice(Gamepad.all[gamepadNum]);
                user3.AssociateActionsWithUser(controls);
                break;
            default:
                controls.Player1.Enable();
                controls.Player1.Torch.performed += torch.Torch;
                controls.Player1.Jump.performed += move.JumpCall;

                foreach (Lever l in lever)
                {
                    controls.Player1.Lever.performed += l.LeverCall;
                }

                break;
        }
    }

    private void OnDisable()
    {
        if (!hasAuthority) return;
        switch (controller)
        {
            case ControllerType.KEYBOARD1:
                controls.Player1.Disable();
                controls.Player1.Torch.performed -= torch.Torch;
                controls.Player1.Attack.performed -= attack.AttackCall;
                controls.Player1.Jump.performed -= move.JumpCall;

                foreach (Lever l in lever)
                {
                    controls.Player1.Lever.performed -= l.LeverCall;
                }

                break;
            case ControllerType.KEYBOARD2:
                controls.Player2.Disable();
                controls.Player2.Torch.performed -= torch.Torch;
                controls.Player2.Attack.performed -= attack.AttackCall;
                controls.Player2.Jump.performed -= move.JumpCall;

                foreach (Lever l in lever)
                {
                    controls.Player2.Lever.performed -= l.LeverCall;
                }

                break;
            case ControllerType.GAMEPAD:
                controls.Player1.Disable();
                controls.Player1.Torch.performed -= torch.Torch;
                controls.Player1.Attack.performed -= attack.AttackCall;
                controls.Player1.Jump.performed -= move.JumpCall;

                foreach (Lever l in lever)
                {
                    controls.Player1.Lever.performed -= l.LeverCall;
                }

                break;
            default:
                controls.Player1.Disable();
                controls.Player1.Torch.performed -= torch.Torch;
                controls.Player1.Jump.performed -= move.JumpCall;

                foreach (Lever l in lever)
                {
                    controls.Player1.Lever.performed -= l.LeverCall;
                }

                break;
        }
    }

    private void Update()
    {
        if (!hasAuthority) return;
        ReadInput();
        if (movementInput.x != 0) { 
            
            GetComponentInChildren<Animator>().SetBool("isMoving", true); 

        } 
        else
        {
            GetComponentInChildren<Animator>().SetBool("isMoving", false);
        }
    }

    private void ReadInput()
    {
        switch (controller)
        {
            case ControllerType.KEYBOARD1:
                movementInput = controls.Player1.Movement.ReadValue<Vector2>();
                break;
            case ControllerType.KEYBOARD2:
                movementInput = controls.Player2.Movement.ReadValue<Vector2>();
                break;
            case ControllerType.GAMEPAD:
                movementInput = controls.Player1.Movement.ReadValue<Vector2>();
                break;
            default:
                movementInput = controls.Player1.Movement.ReadValue<Vector2>();
                break;
        }
    }
}
