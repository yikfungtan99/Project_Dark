using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawnpoint : NetworkBehaviour, ISelectable
{
    [SerializeField] private GameObject spawnPrefab;
    private GameObject spawnedObject;

    private List<GameObject> possibleSpawn = new List<GameObject>();
    private void Start()
    {
        List<GameObject> weaponPickups = new List<GameObject>();
        

        foreach (GameObject weaponObj in WeaponStorageHolder.Instance.WeaponStorage.weapons)
        {
            possibleSpawn.Add(weaponObj.GetComponent<Weapon>().pickup);
        }
        
        
        possibleSpawn.Add(spawnPrefab);
    }

    public void Trigger()
    {
        if (!isServer) return;
        if (spawnedObject != null) return;
        Spawn();
    }

    void Spawn()
    {
        float rand = Random.Range(0.0f, 100.0f);
        int select = 0;

        if(rand < 30.0f)
        {
            select = 1;
        }
        else
        {
            select = 0;
        }

        GameObject spawn = Instantiate(possibleSpawn[select], transform.position, Quaternion.identity);
        NetworkServer.Spawn(spawn);
        spawnedObject = spawn;
    }
}
