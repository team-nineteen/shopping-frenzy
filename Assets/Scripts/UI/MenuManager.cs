using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    const string SFX_STRING = "SFXVolume";
    const string MUS_STRING = "MusicVolume";

    [Tooltip("Root GameObject of the menu used to toggle its activation")]
    public GameObject menuRoot;

    [Tooltip("Slider component for FOV")]
    public Slider fovSlider;
    [Tooltip("Slider component for SFX Volume")]
    public Slider sfxVolumeSlider;
    [Tooltip("Slider component for Music Volume")]
    public Slider musicVolumeSlider;
    [Tooltip("Toggle component for showing debug")]
    public Toggle debugToggle;
    [Tooltip("Button component for showing controls")]
    public Button controlsButton;

    [Header("Control Menu Stuff")]
    [Tooltip("GameObject for the controls")]
    public GameObject controlRoot;
    [Tooltip("Slider component for Sensitivity")]
    public Slider sensitivitySlider;
    [Tooltip("Button component for Toggle Sprint")]
    public Toggle sprintToggle;
    [Tooltip("Button component for Toggle Crouch")]
    public Toggle crouchToggle;
    [Tooltip("Button component for Toggle Interact")]
    public Toggle interactToggle;
    [Tooltip("Button component for going back")]
    public Button backButton;

    PlayerInputHandler m_PlayerInputsHandler;
    Camera m_PlayerCamera;
    DebugView m_DebugView;

    AudioManager m_AudioManager;
    void Start()
    {
        m_PlayerInputsHandler = FindObjectOfType<PlayerInputHandler>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerInputHandler, MenuManager>(m_PlayerInputsHandler, this);

        m_PlayerCamera = FindObjectOfType<PlayerCharacterController>().GetComponentInChildren<Camera>();
        DebugUtility.HandleErrorIfNullFindObject<Camera, MenuManager>(m_PlayerCamera, this);

        m_DebugView = FindObjectOfType<DebugView>();
        DebugUtility.HandleErrorIfNullFindObject<DebugView, MenuManager>(m_DebugView, this);

        m_AudioManager = FindObjectOfType<AudioManager>();
        DebugUtility.HandleErrorIfNullFindObject<AudioManager, MenuManager>(m_AudioManager, this);

        fovSlider.onValueChanged.AddListener(OnFovChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicChanged);
        debugToggle.onValueChanged.AddListener(OnDebugChanged);
        controlsButton.onClick.AddListener(OnControlsClicked);

        // CONTROLS MENU
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        sprintToggle.onValueChanged.AddListener(OnSprintToggle);
        crouchToggle.onValueChanged.AddListener(OnCrouchToggle);
        interactToggle.onValueChanged.AddListener(OnInteractToggle);
        backButton.onClick.AddListener(OnBackButtonClicked);

        UpdateValues();

        menuRoot.SetActive(false);
        controlRoot.SetActive(false);
    }

    private void UpdateValues()
    {
        fovSlider.value = m_PlayerCamera.fieldOfView;
        sfxVolumeSlider.value = m_AudioManager.GetFloat(SFX_STRING);
        musicVolumeSlider.value = m_AudioManager.GetFloat(MUS_STRING);
        debugToggle.isOn = m_DebugView.debugEnabled;
        sensitivitySlider.value = m_PlayerInputsHandler.lookSensitivity;
        sprintToggle.isOn = m_PlayerInputsHandler.sprintToggle;
        crouchToggle.isOn = m_PlayerInputsHandler.crouchToggle;
        interactToggle.isOn = m_PlayerInputsHandler.interactToggle;
    }

    private void Update()
    {
        // Lock cursor when clicking outside of menu
        if (!menuRoot.activeSelf && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetButtonDown(GameConstants.k_ButtonNamePauseMenu)
            || (menuRoot.activeSelf && Input.GetButtonDown(GameConstants.k_ButtonNameCancel)))
        {
            if (controlRoot.activeSelf)
            {
                controlRoot.SetActive(false);
                return;
            }
            SetPauseMenuActivation(!menuRoot.activeSelf);

        }

        if (Input.GetAxisRaw(GameConstants.k_AxisNameVertical) != 0)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                fovSlider.Select();
            }
        }
    }

    public void ClosePauseMenu()
    {
        SetPauseMenuActivation(false);
    }

    public void SetPauseMenuActivation(bool active)
    {
        menuRoot.SetActive(active);

        if (menuRoot.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;

            EventSystem.current.SetSelectedGameObject(null);
            UpdateValues();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    void OnSensitivityChanged(float newValue)
    {
        m_PlayerInputsHandler.lookSensitivity = newValue;
    }

    void OnFovChanged(float newValue)
    {
        m_PlayerCamera.fieldOfView = newValue;
    }

    void OnSFXChanged(float newValue)
    {
        m_AudioManager.SetFloat(SFX_STRING, newValue);
    }
    void OnMusicChanged(float newValue)
    {
        m_AudioManager.SetFloat(MUS_STRING, newValue);
    }
    void OnDebugChanged(bool newValue)
    {
        m_DebugView.ToggleDebug(newValue);
    }
    void OnControlsClicked()
    {
        controlRoot.SetActive(true);
    }

    void OnBackButtonClicked()
    {
        controlRoot.SetActive(false);
    }
    void OnSprintToggle(bool newValue)
    {
        m_PlayerInputsHandler.sprintToggle = newValue;
    }
    void OnCrouchToggle(bool newValue)
    {
        m_PlayerInputsHandler.crouchToggle = newValue;
    }
    void OnInteractToggle(bool newValue)
    {
        m_PlayerInputsHandler.interactToggle = newValue;
    }

}
