using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Lever : Trigger
{
    [SerializeField] private SpriteRenderer s_lever;
    [SerializeField] private Sprite s_leverOn;
    [SerializeField] private Sprite s_leverOff;
    [SerializeField] private PlayerTorch torch;

    public bool triggered;
    public bool activateLever;

    private void ChangeSprite()
    {
        if (!triggered)
        {
            triggered = true;
            s_lever.sprite = s_leverOn;
        }
        else
        {
            triggered = false;
            s_lever.sprite = s_leverOff;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("Player"))
        {
            torch = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTorch>();
            torch.useTorch = false;
            activateLever = true;

            if (torch.torchOn)
            {
                torch.CmdTorchOff();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("Player"))
        {
            torch.useTorch = false;
            activateLever = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("Player"))
        {
            torch.useTorch = true;
            activateLever = false;
        }
    }

    public void LeverCall(CallbackContext ctx)
    {
        if (torch == null) return;

        if (!torch.useTorch && activateLever)
        {
            CmdTrigger();
            ChangeSprite();
        }

        if (!hasAuthority) return;
    }

    [Command(ignoreAuthority = true)]
    public void CmdTrigger()
    {
        RpcFireTrigger();
    }
}
