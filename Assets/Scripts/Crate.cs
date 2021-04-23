using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Pickups
{
    [SerializeField] private GameObject healEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickUp(collision);
    }

    public override void PickUp(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHit")) return;
        if (collision.gameObject.transform.parent.CompareTag("Player"))
        {
            GameObject effect = GameObject.Instantiate(healEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1);
            NetworkServer.Destroy(gameObject);
            collision.transform.parent.gameObject.GetComponent<PlayerStats>().ModifyHealth(1);
        }
    }
}
