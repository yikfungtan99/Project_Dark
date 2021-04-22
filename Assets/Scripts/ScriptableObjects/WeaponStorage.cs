using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Storage", menuName = "ScriptableObjects/Weapon Storage")]
public class WeaponStorage : ScriptableObject
{
    public List<GameObject> weapons;
}
