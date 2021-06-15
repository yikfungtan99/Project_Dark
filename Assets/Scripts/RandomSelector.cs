﻿using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    void Select();
}

public class RandomSelector : NetworkBehaviour
{
    
    [SerializeField] private List<GameObject> objs = new List<GameObject>();
    [SerializeField] private float frequency;

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            CheckForNonSelectable();
            InvokeRepeating("RandomSelect", 2.0f, frequency);
        }
    }

    void CheckForNonSelectable()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            if (objs[i].GetComponent<ISelectable>() == null)
            {
                objs.Remove(objs[i]);
                print(objs + " is not selectable, removing...");
            }
        }
    }

    private void RandomSelect()
    {
        int rand = 0;
        
        if(objs.Count > 0)
        {
            rand = Random.Range(0, objs.Count);
        }
        else
        {
            return;
        }
       
        //target.Trigger();
        RpcTriggerTarget(rand);

    }

    [ClientRpc]
    private void RpcTriggerTarget(int rand)
    {
        ISelectable target = null;

        target = objs[rand].GetComponent<ISelectable>();

        if (target != null)
        {
            //target.Trigger();
            target.Select();
        }
    }
}
