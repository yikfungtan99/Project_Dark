using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testrb : MonoBehaviour, ISelectable
{
    public void Trigger()
    {
        print("HELLO I AM" + gameObject.name);
    }
}
