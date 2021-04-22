using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStorageHolder : MonoBehaviour
{
    private static WeaponStorageHolder _instance;

    public static WeaponStorageHolder Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<WeaponStorageHolder>();
            }

            return _instance;
        }
    }

    public WeaponStorage WeaponStorage;
}
