﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class WhiteSingularity : AbstractSingularity
{
    public static readonly float REPULSE_DELAY = 0.2f;

    #region Fields
    [SerializeField] private WhiteSingularityData _data;

    private bool _canRepulse = true;
    #endregion

    #region Overrided Methods
    protected override void OnEnter()
    {
        RepulsePlayer();
    }

    protected override void OnStay() {}
    #endregion

    private void RepulsePlayer()
    {
        if (!_canRepulse)
            return;

        _canRepulse = false;
        StartCoroutine(CustomDelay.ExecuteAfterTime(REPULSE_DELAY, () =>
        {
            _canRepulse = true;
        }));

        Vector3 dir = (_character.position - transform.position).normalized;

        // apply velocity
        Vector3 vel = dir * _data.RepulsionDistance;
        _character.GetComponent<Rigidbody>().velocity = vel;

        // debug
        GizmosPersistence.DrawPersistentLine(transform.position, transform.position + vel);
    }

    void OnEnable()
    {
        _canRepulse = true;
    }
}
