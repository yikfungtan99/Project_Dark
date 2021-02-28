using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    [SyncVar(hook = "DamageEffects")] public int health = 10;

    public int playerNum = 0;
    public bool alive = true;

    public void ModifyHealth(int amount)
    {
        health += amount;
    }

    private void DamageEffects(int oldValue, int newValue)
    {
        if(oldValue > newValue)
        {
            if(newValue <= 0)
            {
                Death();
            }
        }
        else
        {

        }
    }

    private void Death()
    {
        alive = false;
        gameObject.SetActive(false);
        GameManager.Instance.CheckerPlayersRemaining();

    }
}
