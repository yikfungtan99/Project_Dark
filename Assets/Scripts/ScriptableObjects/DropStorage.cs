using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drop Storage", menuName = "ScriptableObjects/Drop Storage")]
public class DropStorage : ScriptableObject
{
    public List<GameObject> drops;
    public List<GameObject> equippable;
}
