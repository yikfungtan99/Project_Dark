using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    [SyncVar(hook = "DamageEffects")] public int health = 10;

    [SyncVar] public int playerNum = 0;

    [SerializeField] private float gracePeriod = 0.5f;

    public bool alive = true;
    public bool grace = false;

    [SerializeField] private ParticleSystem healEffect;

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

            if(!grace) StartCoroutine(Grace());

            if(newValue <= 0)
            {
                //CmdDeath();
                Death();
            }
        }
        else
        {
            healEffect.Play();
        }
    }

    IEnumerator Grace()
    {
        print("GRACE STARTED");
        grace = true;
        yield return new WaitForSeconds(gracePeriod);
        print("GRACE ENDED");
        grace = false;
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
