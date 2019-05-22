﻿using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class BlackSingularity : AbstractSingularity
{
    #region Fields
    [SerializeField] private BlackSingularityData _data;
    [SerializeField] private DrawCircle _rangeFeedback;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        GetComponent<SphereCollider>().radius = _data.Radius;
        UpdateRangeFeedback();
    }
    #endregion

    #region Overrided Methods
    protected override void OnStay()
    {
        if (Vector3.Distance(transform.position, _character.position) <= _data.CharacterRotateRadius)
        {
            _character.SetParent(transform);
            _character.GetComponent<CharControllerSingularity>().RotateAroundSingularity(transform);
        }
        else
        {
            AttractPlayer();
        }
    }

    protected override void OnExit()
    {
        //throw new System.NotImplementedException();
    }
    #endregion

    private void AttractPlayer()
    {
        Vector3 dir = (transform.position - _character.position).normalized;
        float speed = _data.Radius / _data.TimeToReachCenter;

        // apply velocity
        Vector3 vel = speed * dir;
        _character.GetComponent<Rigidbody>().velocity = vel * Time.deltaTime;
    }

    public void UpdateRangeFeedback()
    {
        if (_rangeFeedback == null)
        {
            Debug.LogError(transform.name + " has no range feedback dragged!");
            return;
        }

        _rangeFeedback.xradius = _data.Radius;
        _rangeFeedback.yradius = _data.Radius;

        _rangeFeedback.UpdateCircle();
    }
}
