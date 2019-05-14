﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteSingularity : MonoBehaviour
{
    #region Fields
    [SerializeField] private WhiteSingularityData _data;

    private Transform _character;
    #endregion

    #region MonoBehaviour Callbacks
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _character = other.transform;

            RepulsePlayer();
        }
    }
    #endregion

    private void RepulsePlayer()
    {
        Vector3 dir = (_character.position - transform.position).normalized;

        // apply velocity
        Vector3 vel = _character.GetComponent<Rigidbody>().velocity;

        vel.x = dir.x * _data.RepulsionDistance;
        vel.y = dir.y * _data.RepulsionDistance;

        _character.GetComponent<Rigidbody>().velocity = vel;

        // debug
        GizmosPersistence.DrawPersistentLine(transform.position, transform.position + vel);
    }
}
