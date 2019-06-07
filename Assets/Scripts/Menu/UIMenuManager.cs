﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void LanguageHandle();

public class UIMenuManager : MonoBehaviour
{
    #region Enums
    private enum PanelType
    {
        MainMenu,
        OptionsGeneral,
        OptionsSound,
        OptionsQuality,
        Credits
    };
    #endregion

    #region Fields
    #region SerializeFields
    [SerializeField] private EventSystem _eventSystem;
    [Header("Panels")]
    [SerializeField] private GameObject _panelMainMenu;
    [SerializeField] private GameObject _panelOptionsGeneral;
    [SerializeField] private GameObject _panelOptionsSound;
    [SerializeField] private GameObject _panelOptionsQuality;
    [SerializeField] private GameObject _panelCredits;
    [Header("Main Menu")]
    [SerializeField] private Button _buttonPlay;
    [SerializeField] private Button _buttonOptions;
    [SerializeField] private Button _buttonCredits;
    [SerializeField] private Button _buttonQuit;
    [Space]
    [SerializeField] private TMP_Dropdown _dropdownLanguage;
    [Header("Quality Panel")]
    [SerializeField] private Button _buttonQuality;
    [SerializeField] private Button _buttonSound;
    [Space]
    [SerializeField] private Toggle _toggleVSync;
    [SerializeField] private TMP_Dropdown _dropdownFullscreen;
    [Header("Returns buttons")]
    [SerializeField] private List<Button> _buttonsReturn = new List<Button>();
    #endregion

    #region Public static fields
    public static LanguageHandle EventLanguageChangement;
    #endregion

    #region Privates Fields
    private PanelType _currentPanel = PanelType.MainMenu;
    private AsyncOperation _ao;
    #endregion
    #endregion

    void Awake()
    {
        LoadSettings();
        DisplayPanel(PanelType.MainMenu);

        // main menu
        _buttonPlay.onClick.AddListener(LoadTutorial);  // load tutorial
        _buttonOptions.onClick.AddListener(() => DisplayPanel(PanelType.OptionsGeneral));
        _buttonCredits.onClick.AddListener(() => DisplayPanel(PanelType.Credits));
        _buttonQuit.onClick.AddListener(Application.Quit);

        _dropdownLanguage.onValueChanged.AddListener(DropdownLanguageChanged);

        // quality panel
        _buttonQuality.onClick.AddListener(() => DisplayPanel(PanelType.OptionsQuality));
        _buttonSound.onClick.AddListener(() => DisplayPanel(PanelType.OptionsSound));

        // return buttons
        foreach (Button b in _buttonsReturn)
        {
            b.onClick.AddListener(ReturnButtonPressed);
            b.onClick.AddListener(() => AudioManager.Instance.PlaySoundUI(SoundUI.Button));
        }

        // set audio for every buttons
        var buttons = FindObjectsOfType<Button>();

        foreach (Button b in buttons)
        {
            b.onClick.AddListener(() => AudioManager.Instance.PlaySoundUI(SoundUI.Button));
        }
    }

    void Start()
    {
        // doesn't work in Awake()
        _ao = SceneManager.LoadSceneAsync(SceneState.Tutorial.ToScene());
        _ao.allowSceneActivation = false;
    }

    void LoadTutorial()
    {
        if (_ao != null)
        {
            _ao.allowSceneActivation = true;
        }
    }

    void DisplayPanel(PanelType panel)
    {
        _currentPanel = panel;

        _panelMainMenu.SetActive(false);
        _panelOptionsGeneral.SetActive(false);
        _panelOptionsSound.SetActive(false);
        _panelOptionsQuality.SetActive(false);
        _panelCredits.SetActive(false);

        switch (panel)
        {
            case PanelType.MainMenu:
                _panelMainMenu.SetActive(true);
                _eventSystem.SetSelectedGameObject(_buttonPlay.gameObject);
                break;

            case PanelType.OptionsGeneral:
                _panelOptionsGeneral.SetActive(true);
                _eventSystem.SetSelectedGameObject(_buttonQuality.gameObject);
                break;

            case PanelType.OptionsSound:
                _panelOptionsSound.SetActive(true);
                //_eventSystem.firstSelectedGameObject = _buttonQuality.gameObject;    
                break;

            case PanelType.OptionsQuality:
                _panelOptionsQuality.SetActive(true);
                _eventSystem.SetSelectedGameObject(_toggleVSync.gameObject);
                break;

            case PanelType.Credits:
                _panelCredits.SetActive(true);
                _eventSystem.SetSelectedGameObject(_buttonsReturn[3].gameObject);
                break;
        }
    }

    void ReturnButtonPressed()
    {
        switch (_currentPanel)
        {
            case PanelType.Credits:
            case PanelType.OptionsGeneral:
                DisplayPanel(PanelType.MainMenu);
                break;

            case PanelType.OptionsSound:
            case PanelType.OptionsQuality:
                DisplayPanel(PanelType.OptionsGeneral);
                SaveSettings();
                break;
        }
    }

    void DropdownLanguageChanged(int i)
    {
        SaveSettings();
        Translation.ResetTranslations();

        EventLanguageChangement?.Invoke();
    }

    #region Load/Save Settings Methods
    void LoadSettings()
    {
        _toggleVSync.isOn = SaveSystem.OptionsData.enableVSync;
        _dropdownFullscreen.SetValueWithoutNotify(SaveSystem.OptionsData.FullScreenValue);

        _dropdownLanguage.value = SaveSystem.OptionsData.Language;
    }

    void SaveSettings()
    {
        SaveSystem.OptionsData.enableVSync = _toggleVSync.isOn;
        SaveSystem.OptionsData.FullScreenValue = _dropdownFullscreen.value;

        SaveSystem.OptionsData.Language = _dropdownLanguage.value;

        SaveSystem.Save();
        Debug.Log("SaveSettings: " + SaveSystem.OptionsData.language);
    }
    #endregion
}
