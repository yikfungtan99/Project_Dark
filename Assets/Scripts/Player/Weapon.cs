using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject pickup;
    public float charges;

    public virtual void ConsumeCharges()
    {
        charges -= 1;

        if(charges <= 0)
        {
            transform.parent.GetComponentInParent<PlayerWeaponHolder>().UnEquip();
        }
    }
}
