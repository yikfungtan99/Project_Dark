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
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHit")) return;
        if (collision.gameObject.transform.parent.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponentInParent<PlayerWeaponHolder>() != null)
            {
                collision.gameObject.GetComponentInParent<PlayerWeaponHolder>().Equip(weapon);
                NetworkServer.Destroy(gameObject);
            }
        }
    }
}
