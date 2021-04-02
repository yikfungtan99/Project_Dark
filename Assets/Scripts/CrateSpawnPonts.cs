using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawnPonts : NetworkBehaviour, ISelectable
{
    [SerializeField] private GameObject spawnPrefab;
    private GameObject spawnedCrate;

    public void Trigger()
    {
        if (!isServer) return;
        if (spawnedCrate != null) return;
        Spawn();
    }

    void Spawn()
    {
        GameObject spawn = GameObject.Instantiate(spawnPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(spawn);
        spawnedCrate = spawn;
    }
}
