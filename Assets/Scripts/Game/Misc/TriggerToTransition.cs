﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerToTransition : MonoBehaviour
{
    #region Fields
    [SerializeField] private bool _hasBeenTriggered = false;
    #endregion

    #region MonoBehaviour Callbacks
    private void OnTriggerEnter(Collider other)
    {
        if (_hasBeenTriggered)
            return;

        if (other.CompareTag("Player"))
        {
            _hasBeenTriggered = true;

            SingletonExtension.ResetSingleton();

            GameState.state++;
            Initiate.Fade("transition", Color.black, 1f);

            GameState.CurrentSpeedrunTime = Time.timeSinceLevelLoad;
        }
    }
    #endregion
}
