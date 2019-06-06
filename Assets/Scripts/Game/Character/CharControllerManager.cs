﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControllerManager : Singleton<CharControllerManager>
{
    private bool _attracted = false;

    public bool Attracted
    {
        get
        {
            return _attracted;
        }

        set
        {
            _attracted = value;

            GetComponent<CharController>().enabled = !_attracted;
            GetComponent<Rigidbody>().useGravity = !_attracted;

            if (_attracted)
            {
                GetComponent<CharController>().ResetMovements();                
            }
        }
    }

    #region MonoBehaviour Callbacks
    void Start()
    {
        // on death, set attracted to false
        DeathHandle d = new DeathHandle(() =>
        {
            Attracted = false;
            GameState.CurrentDeathCount++;
        });
        CharDeath.EventDeath += d;

        // on form change, set parent to null
        FormHandle dd = new FormHandle((Form form) =>
        {
            transform.parent = null;
            Attracted = false;
        });
        CharControllerSingularity.EventForm += dd;
    }

    void Update()
    {
        transform.eulerAngles = Vector3.zero;
    }
    #endregion
}
