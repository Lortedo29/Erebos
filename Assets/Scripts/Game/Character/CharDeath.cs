﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeathHandle(object sender);
public delegate void RespawnHandle(object sender);

public class CharDeath : MonoBehaviour
{
    public static readonly int DEATH_Y = -10;
    public static readonly float RESPAWN_TIME = 2f;

    #region Fields
    public static event DeathHandle EventDeath;
    public static event RespawnHandle EventRespawn;

    [HideInInspector] public Vector3 currentCheckpoint;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        currentCheckpoint = transform.position;

        // call respawn event with delay
        DeathHandle d1 = new DeathHandle(InvokeRespawn);
        EventDeath += d1;
    }

    void Update()
    {
        if (transform.position.y <= DEATH_Y)
        {
            Death();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KillingObstacle"))
        {
            Death();
        }

        if (other.CompareTag("Checkpoint"))
        {
            currentCheckpoint = other.transform.position;
            other.GetComponent<Checkpoint>().ActiveBrasero();
        }
    }

    void OnDestroy()
    {
        EventDeath = null;
        EventRespawn = null;
    }
    #endregion

    public void Death()
    {
        AudioManager.Instance.PlaySoundGeneral(SoundGeneral.Death);

        EventDeath?.Invoke(this);
    }

    private void InvokeRespawn(object sender)
    {
        StartCoroutine(CustomDelay.ExecuteAfterTime(RESPAWN_TIME, () =>
        {
            transform.position = (Vector2)currentCheckpoint;

            EventRespawn?.Invoke(this);
        }));
    }
}
