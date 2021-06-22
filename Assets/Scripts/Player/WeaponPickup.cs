using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickups
{
    [SerializeField] private GameObject weapon;

    private void OnTriggerStay2D(Collider2D collision)
    {
        PickUp(collision);
    }

    public override void PickUp(Collider2D collision)
    {
        if (!isServer) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHit")) return;
        if (collision.gameObject.transform.parent.CompareTag("Player"))
        {
            PlayerWeaponHolder playerWeaponHolder = collision.gameObject.GetComponentInParent<PlayerWeaponHolder>();

            if (playerWeaponHolder != null)
            {
                if(playerWeaponHolder.holdingWeapon == null)
                {
                    playerWeaponHolder.Equip(weapon);
                    NetworkServer.Destroy(gameObject);
                }
            }
        }
    }
}
