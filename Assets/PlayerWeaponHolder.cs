﻿using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHolder : NetworkBehaviour
{
    [SerializeField] private Transform playerWeaponHolder;
    [SerializeField] private SpriteRenderer mainSprite;

    public Weapon holdingWeapon;

    private SpriteRenderer sprite;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(sprite != null)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, mainSprite.color.a);
            sprite.enabled = mainSprite.enabled;
        }
    }

    public void Equip(int weaponIndex)
    {
        RpcEquip(weaponIndex);
    }

    [ClientRpc]
    public void RpcEquip(int weaponIndex)
    {
        if (holdingWeapon) return;
        GameObject weaponRef = WeaponStorageHolder.Instance.WeaponStorage.weapons[weaponIndex];
        GameObject weaponObj = Instantiate(weaponRef, playerWeaponHolder);
        Weapon weapon = weaponObj.GetComponent<Weapon>();
        sprite = weapon.gameObject.GetComponent<SpriteRenderer>();
        holdingWeapon = weapon;
        AudioManager.Instance.Play("Draw");
    }

    public void UnEquip()
    {
        CmdUnEquip();
    }

    [Command]
    public void CmdUnEquip()
    {
        RpcUnEquip();
    }

    [ClientRpc]
    public void RpcUnEquip()
    {
        if (!holdingWeapon) return;
        Destroy(holdingWeapon.gameObject);
        holdingWeapon = null;
        AudioManager.Instance.Play("Break");
    }
}
