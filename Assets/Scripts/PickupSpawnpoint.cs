using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawnpoint : NetworkBehaviour, ISelectable
{
    private GameObject spawnedObject;

    private List<GameObject> possibleSpawn = new List<GameObject>();
    private void Start()
    {
        List<GameObject> weaponPickups = new List<GameObject>();
        

        foreach (GameObject dropObj in DropStorageHolder.Instance.dropStorage.drops)
        {
            possibleSpawn.Add(dropObj);
        }
    }

    public void Select()
    {
        if (!isServer) return;
        if (spawnedObject != null) return;
        Spawn();
    }

    void Spawn()
    {
        int rand = Random.Range(0, possibleSpawn.Count);

        GameObject spawn = Instantiate(possibleSpawn[2], transform.position, Quaternion.identity);
        NetworkServer.Spawn(spawn);
        spawnedObject = spawn;
    }
}
