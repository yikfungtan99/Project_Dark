using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    [SyncVar(hook = "DamageEffects")] public int health = 10;

    [SyncVar] public int playerNum = 0;
    public bool alive = true;

    private HUDManager hud;

    public void Start()
    {
        StartCoroutine(AddToAvatarList());
        hud = GameManager.Instance.hud;
    }

    IEnumerator AddToAvatarList()
    {
        yield return new WaitForSeconds(0.02f);
        GameManager.Instance.AddToAvatarList(gameObject);
        GameManager.Instance.hud.UpdatePlayerHUD(this);
    }

    public void ModifyHealth(int amount)
    {
        health += amount;
    }

    private void DamageEffects(int oldValue, int newValue)
    {
        //TAKE DAMAGE

        if(oldValue > newValue)
        {
            if(newValue <= 0)
            {
                CmdDeath();
            }
        }
        else
        {

        }
    }

    private void Death()
    {
        print("Death");
        alive = false;
        gameObject.SetActive(false);

        GameManager.Instance.CheckForPlayerRemaining();
    }

    [Command(ignoreAuthority = true)]
    private void CmdDeath()
    {
        RpcDeath();
    }

    [ClientRpc]
    private void RpcDeath()
    {
        Death();
    }
}
