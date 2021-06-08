using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    public int maxHealth;
    [SyncVar(hook = "DamageEffects")] public int health;
    [SyncVar(hook = "ChargeEffects")] public int battery;

    [SyncVar] public int playerNum = 0;

    [SerializeField] private float gracePeriod = 0.5f;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private PlayerTorch torch;

    public bool alive = true;
    public bool grace = false;
    public bool reveal = false;
     
    private HUDManager hud;

    public void Start()
    {
        StartCoroutine(AddToAvatarList());
        hud = GameManager.Instance.hud;

        if (health != maxHealth) health = maxHealth;
    }

    private void Update()
    {
        if (GlobalLight.Instance.globalLite.intensity > GlobalLight.Instance.targetDarkness)
        {
            if (GlobalLight.Instance.globalLite.intensity > 0.05) sprite.color = new Color(sprite.color.r, sprite.color.b, sprite.color.b, GlobalLight.Instance.globalLite.intensity * 1.5f);
            if (reveal)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.b, sprite.color.b, 255);
            }
        }
        else
        {
            sprite.enabled = reveal;
            sprite.color = new Color(sprite.color.r, sprite.color.b, sprite.color.b, 255);
        }

        reveal = false;
    }

    IEnumerator AddToAvatarList()
    {
        yield return new WaitForSeconds(0.02f);
        GameManager.Instance.AddToAvatarList(gameObject);
        GameManager.Instance.hud.UpdatePlayerHUD(this);
    }

    public void ModifyHealth(int amount)
    {
        CmdModifyHealth(amount);
    }

    [Command(ignoreAuthority = true)]
    public void CmdModifyHealth(int amount)
    {
        health += amount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health < 0) health = 0;
    }

    private void DamageEffects(int oldValue, int newValue)
    {
        if (newValue > oldValue)
        {
            print("heal");
        }

        //TAKE DAMAGE
        if (oldValue > newValue)
        {
            if(!grace) StartCoroutine(Grace());

            if(newValue <= 0)
            {
                Death();
            }
        }
    }

    public void ChargeBattery(int amnt)
    {
        CmdChargeBattery(amnt);
    }

    [Command(ignoreAuthority = true)]
    public void CmdChargeBattery(int amnt)
    {
        torch.battery += amnt;

        if (torch.battery >= torch.maxTorchBattery) torch.battery = torch.maxTorchBattery;
    }

    private void ChargeEffects(int oldValue, int newValue)
    {
        if (newValue > oldValue)
        {
            print("charge");
        }
    }

    IEnumerator Grace()
    {
        grace = true;
        yield return new WaitForSeconds(gracePeriod);
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
