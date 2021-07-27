using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrateType
{
    HEAL,
    BATTERY
}

public class Crate : Pickups
{
    [SerializeField] private CrateType crateType;
    [SerializeField] private GameObject pickupEffect;
    [SerializeField] private int effectAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickUp(collision);
    }

    public override void PickUp(Collider2D collision)
    {
        if (!isServer) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHit")) return;
        if (collision.gameObject.transform.parent.CompareTag("Player"))
        {
            if (pickupEffect != null)
            {
                GameObject effect = GameObject.Instantiate(pickupEffect, transform.position, Quaternion.identity);
                Destroy(effect, 1);
            }

            switch (crateType)
            {
                case CrateType.HEAL:

                    collision.transform.parent.gameObject.GetComponent<PlayerStats>().ModifyHealth(effectAmount);
                    break;

                case CrateType.BATTERY:

                    collision.transform.parent.gameObject.GetComponent<PlayerStats>().ChargeBattery(effectAmount);
                    break;

                default:
                    break;
            }

            NetworkServer.Destroy(gameObject);
        }
    }
}
