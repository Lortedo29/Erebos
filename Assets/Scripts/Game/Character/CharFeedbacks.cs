﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharFeedbacks : Singleton<CharFeedbacks>
{
    #region Fields
    [SerializeField] private GameObject _model;
    [SerializeField] private GameObject _blackMask;
    [Space]
    [SerializeField] private GameObject _prefabJumpPS;
    [Header("Form")]
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [Space]
    [SerializeField] private Material _materialBlackForm;
    [SerializeField] private Material _materialWhiteForm;
    [Space]
    [SerializeField] private GameObject _prefabBlackFormPS;
    [SerializeField] private GameObject _prefabWhiteFormPS;
    [Header("Death")]
    [SerializeField] private GameObject _prefabDeathPS;
    [SerializeField] private GameObject _prefabRespawnPS;
    [Header("Dash")]
    [SerializeField] private GameObject _prefabBurstDash;
    [SerializeField] private GameObject _prefabHeadDash;
    [SerializeField] private GameObject _prefabEndDash;

    private bool _isDashing = false;

    // cached variables
    private CharControllerSingularity _charControllerSingularity = null;
    #endregion

    #region Properties
    public GameObject BlackMask { get => _blackMask; }
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        DeathHandle d1 = new DeathHandle(PlayDeath);
        CharDeath.EventDeath += d1;

        RespawnHandle d2 = new RespawnHandle(PlayRespawn);
        CharDeath.EventRespawn += d2;

        FormHandle d3 = new FormHandle(PlayFormChange);
        CharControllerSingularity.EventForm += d3;

        _charControllerSingularity = GetComponent<CharControllerSingularity>();
    }

    void Update()
    {
        bool isInBlackSingularity = CharControllerManager.Instance.Attracted && _charControllerSingularity.Form == Form.Void;

        bool shouldBeActive = !(_isDashing || CharDeath.isDead || isInBlackSingularity);

        _model.SetActive(shouldBeActive);
        _blackMask.SetActive(isInBlackSingularity);
    }
    #endregion

    public void PlayJumpPS()
    {
        Instantiate(_prefabJumpPS, transform.position, Quaternion.identity);
    }

    void PlayFormChange(object sender, Form form)
    {
        switch (form)
        {
            case Form.Ethereal:
                var obj = Instantiate(_prefabWhiteFormPS, transform.position, Quaternion.identity);
                obj.transform.parent = transform;

                _meshRenderer.material = _materialWhiteForm;
                break;

            case Form.Void:
                obj = Instantiate(_prefabBlackFormPS, transform.position, Quaternion.identity);
                obj.transform.parent = transform;

                _meshRenderer.material = _materialBlackForm;
                break;
        }
    }

    #region Death & Respawn
    void PlayDeath(object sender)
    {
        Instantiate(_prefabDeathPS, transform.position, Quaternion.identity);
    }

    void PlayRespawn(object sender)
    {
        Instantiate(_prefabRespawnPS, transform.position, Quaternion.identity);
    }
    #endregion

    #region Dash Sequence
    public void PlayDashSequence(float angle)
    {
        Debug.Log("Dash Sequence!");

        Vector3 rotation = Quaternion.identity.eulerAngles + new Vector3(0, 0, angle);
        Instantiate(_prefabBurstDash, transform.position, Quaternion.Euler(rotation));

        GameObject obj = Instantiate(_prefabHeadDash, transform.position, Quaternion.Euler(rotation));
        obj.transform.parent = transform;

        _isDashing = true;
    }

    public void StopDashSequence()
    {
        Instantiate(_prefabEndDash, transform.position + Vector3.back * 3f, Quaternion.identity);

        _isDashing = false;
    }
    #endregion
}
