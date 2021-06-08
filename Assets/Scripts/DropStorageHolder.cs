using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropStorageHolder : MonoBehaviour
{
    private static DropStorageHolder _instance;

    public static DropStorageHolder Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DropStorageHolder>();
            }

            return _instance;
        }
    }

    public DropStorage dropStorage;
}
