﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testrb : MonoBehaviour, ISelectable
{
    public void Select()
    {
        print("HELLO I AM" + gameObject.name);
    }
}
