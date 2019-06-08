﻿using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : Singleton<TransitionManager>
{
    #region Fields
#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool _enableDebug = false;
    [SerializeField] private SceneState _scene;
#endif
    [Header("Panel linkage")]
    [SerializeField] private TextMeshProUGUI _dialogue;
    [SerializeField] private Image[] _image = new Image[2];
    [Header("Transitions data")]
    [SerializeField] private List<Transition> _transitionsZoneOne = new List<Transition>();
    [SerializeField] private List<Transition> _transitionsZoneTwo = new List<Transition>();
    [SerializeField] private List<Transition> _transitionsEnd = new List<Transition>();

    private Dictionary<SceneState, List<Transition>> _transitions = new Dictionary<SceneState, List<Transition>>();
    private AsyncOperation ao;

    private int _currentTransitionIndex = -1;
    private Transition _currentTransition;

    private int _alternator = 9999;
    #endregion

    #region Properties
    private int Alternator
    {
        get
        {
            _alternator++;

            if (_alternator > 1)
                _alternator = 0;

            Debug.Log("Alternator return: " + _alternator);

            return _alternator;
        }
    }
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        _transitions.Add(SceneState.ZoneOne, _transitionsZoneOne);
        _transitions.Add(SceneState.ZoneTwo, _transitionsZoneTwo);
        _transitions.Add(SceneState.End, _transitionsEnd);

        _image[1].CrossFadeAlpha(0, 0, true);

#if UNITY_EDITOR
        if (_enableDebug)
        {
            GameState.currentScene = _scene;
        }
#endif
    }

    void Start()
    {
        string sceneToLoad = GameState.currentScene.ToScene();

        ao = SceneManager.LoadSceneAsync(sceneToLoad);
        ao.allowSceneActivation = false;

        ChangeTransition();
    }

    void Update()
    {
        if (GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One) || Input.GetKeyDown(KeyCode.Space))
        {
            if (_currentTransition != null)
            {
                _currentTransition.ChangeDialogue();
            }
        }
    }
    #endregion

    public void ChangeTransition()
    {
        _currentTransitionIndex++;

        if (_currentTransitionIndex < _transitions[GameState.currentScene].Count)
        {
            if (_currentTransition != null)
            {
                _currentTransition.UnloadVignette();                
            }

            _currentTransition = _transitions[GameState.currentScene][_currentTransitionIndex];
            _currentTransition.LoadVignette(_image[Alternator], _dialogue);
        }
        else
        {
            if (ao != null)
            {
                ao.allowSceneActivation = true;
            }
        }
    }

}
