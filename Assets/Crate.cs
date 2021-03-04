using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : NetworkBehaviour
{
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHit")) return;
        if (collision.gameObject.transform.parent.CompareTag("Player"))
        {
            NetworkServer.Destroy(gameObject);
            collision.transform.parent.gameObject.GetComponent<PlayerStats>().ModifyHealth(3);
        }
    }
}
