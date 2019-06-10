﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerToTransition : MonoBehaviour
{
    #region Fields
    private bool _hasBeenTriggered = false;
    private AsyncOperation _ao = null;
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        _ao = SceneManager.LoadSceneAsync("SC_transition");
        _ao.allowSceneActivation = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (_hasBeenTriggered)
            return;

        if (other.CompareTag("Player"))
        {
            _hasBeenTriggered = true;

            GameState.UpdateCurrentSpeedrunTime();
            GameState.currentScene++;

            this.ExecuteAfterTime(1f, () =>
            {
                Initiate.Fade(_ao, Color.black, 1f);
            });
        }
    }
    #endregion
}
