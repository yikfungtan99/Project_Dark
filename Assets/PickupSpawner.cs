using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : NetworkBehaviour
{
    [SerializeField] private List<PickupSpawnpoint> pickupSpawnPoints = new List<PickupSpawnpoint>();
    [SerializeField] private float repeatRate;

    private List<GameObject> possibleSpawn = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (isClientOnly) return;
        foreach (GameObject dropObj in DropStorageHolder.Instance.dropStorage.drops)
        {
            possibleSpawn.Add(dropObj);
        }

        InvokeRepeating("SpawnPickups", 2.0f, repeatRate);
    }

    void SpawnPickups()
    {
        if (isClientOnly) return;
        int randSpawn = Random.Range(0, possibleSpawn.Count);
        int randPoint = Random.Range(0, pickupSpawnPoints.Count);

        bool full = true;

        foreach (var pickupPoint in pickupSpawnPoints)
        {
            if (pickupPoint.spawnedObject == null)
            {
                full = false;
            }
        }

        if (full)
        {
            print("All spawnpoints occupied!");
            return;
        }

        if(pickupSpawnPoints[randPoint].spawnedObject == null)
        {
            GameObject spawn = Instantiate(possibleSpawn[randSpawn], pickupSpawnPoints[randPoint].transform.position, Quaternion.identity);
            NetworkServer.Spawn(spawn);
            pickupSpawnPoints[randPoint].spawnedObject = spawn;
        }
        else
        {
            SpawnPickups();
        }
    }
}
