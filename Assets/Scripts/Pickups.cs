using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickups : NetworkBehaviour
{
    public abstract void PickUp(Collider2D collision);
}
