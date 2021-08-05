using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonMode
{
    STEP,
    STAY
}

public class Lever : Trigger
{
    [SerializeField] private ButtonMode mode;
    [SerializeField] private SpriteRenderer s_lever;
    [SerializeField] private Sprite s_leverOn;
    [SerializeField] private Sprite s_leverOff;

    public bool triggered;

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
        if (isServer) RpcFireTrigger();

        ChangeSprite();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("Player"))
        {
            if (mode == ButtonMode.STAY)
            {
                if (isServer) RpcFireTrigger();
            }
        }
    }
}
