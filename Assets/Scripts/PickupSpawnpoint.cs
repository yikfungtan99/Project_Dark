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
        int rand = Random.Range(0, possibleSpawn.Count);
        GameObject spawn = Instantiate(possibleSpawn[rand], transform.position, Quaternion.identity);
        NetworkServer.Spawn(spawn);
        spawnedObject = spawn;
    }
}
