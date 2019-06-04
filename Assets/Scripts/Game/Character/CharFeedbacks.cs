﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharFeedbacks : Singleton<CharFeedbacks>
{
    #region Fields
    [SerializeField] private GameObject _model;
    [Space]
    [SerializeField] private GameObject _prefabTrail;
    [SerializeField] private GameObject _prefabJumpPS;
    [Header("Death")]
    [SerializeField] private GameObject _prefabDeathPS;
    [SerializeField] private GameObject _prefabRespawnPS;
    [Header("Dash")]
    [SerializeField] private GameObject _prefabBurstDash;
    [SerializeField] private GameObject _prefabHeadDash;
    [SerializeField] private GameObject _prefabEndDash;

    private Coroutine _coroutineDashSequence = null;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        var trail = Instantiate(_prefabTrail, transform.position, Quaternion.identity);
        trail.GetComponent<FollowTransform>().transformToFollow = transform;

        DeathHandle d1 = new DeathHandle(PlayDeath);
        CharDeath.EventDeath += d1;

        RespawnHandle d2 = new RespawnHandle(PlayRespawn);
        CharDeath.EventRespawn += d2;
    }
    #endregion

    public void PlayJumpPS()
    {
        Instantiate(_prefabJumpPS, transform.position, Quaternion.identity);
    }

    void PlayDeath(object sender)
    {
        _model.SetActive(false);
        Instantiate(_prefabDeathPS, transform.position, Quaternion.identity);

        if (_coroutineDashSequence != null)
        {
            StopCoroutine(_coroutineDashSequence);
        }
    }

    void PlayRespawn(object sender)
    {
        _model.SetActive(true);
        Instantiate(_prefabRespawnPS, transform.position, Quaternion.identity);
    }

    public void PlayDashSequence(float angle)
    {
        Debug.Log("Dash Sequence!");

        Vector3 rotation = Quaternion.identity.eulerAngles + new Vector3(0, 0, angle);
        Instantiate(_prefabBurstDash, transform.position, Quaternion.Euler(rotation));

        _model.SetActive(false);

        GameObject obj = Instantiate(_prefabHeadDash, transform.position, Quaternion.Euler(rotation));
        obj.transform.parent = transform;
    }

    public void StopDashSequence()
    {
        Instantiate(_prefabEndDash, transform.position + Vector3.back * 3f, Quaternion.identity);

        _coroutineDashSequence = StartCoroutine(CustomDelay.ExecuteAfterTime(0.2f, () => _model.SetActive(true)));
    }
}
