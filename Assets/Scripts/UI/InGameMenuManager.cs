using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class InGameMenuManager : MonoBehaviour
{


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
    [Tooltip("Button component for going back to game/settings menu")]
    public Button returnToGameButton;
    [Tooltip("Button component for going back to main menu")]
    public Button backToMenuButton;

    [Tooltip("Whether to apply certain game-pause, cursor-lock mechanics")]
    public bool isInGame = true;

    SettingsData m_SettingsData;

    void Start()
    {

        m_SettingsData = SettingsData.Instance;

        fovSlider.onValueChanged.AddListener(OnFovChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicChanged);
        debugToggle.onValueChanged.AddListener(OnDebugChanged);
        controlsButton.onClick.AddListener(OnControlsClicked);
        returnToGameButton.onClick.AddListener(OnReturnToGameButtonClicked);

        // CONTROLS MENU
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        sprintToggle.onValueChanged.AddListener(OnSprintToggle);
        crouchToggle.onValueChanged.AddListener(OnCrouchToggle);
        interactToggle.onValueChanged.AddListener(OnInteractToggle);
        backButton.onClick.AddListener(OnBackButtonClicked);
        backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);

        UpdateValues();

        if (!isInGame) returnToGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Return to Menu";
        backToMenuButton.gameObject.SetActive(isInGame);
        menuRoot.SetActive(false);
        controlRoot.SetActive(false);
    }

    private void UpdateValues()
    {
        fovSlider.value = m_SettingsData.fov;
        sfxVolumeSlider.value = m_SettingsData.sfxVolume;
        musicVolumeSlider.value = m_SettingsData.musicVolume;
        debugToggle.isOn = m_SettingsData.debugEnabled;
        sensitivitySlider.value = m_SettingsData.mouseSensitivity;
        sprintToggle.isOn = m_SettingsData.toggleSprint;
        crouchToggle.isOn = m_SettingsData.toggleCrouch;
        interactToggle.isOn = m_SettingsData.toggleInteract;
    }

    private void Update()
    {
        // Lock cursor when clicking outside of menu
        if (isInGame && !menuRoot.activeSelf && Input.GetMouseButtonDown(0))
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

    public void SetPauseMenuActivation(bool active)
    {
        menuRoot.SetActive(active);

        if (menuRoot.activeSelf)
        {
            if (isInGame)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
            }
            EventSystem.current.SetSelectedGameObject(null);
            UpdateValues();
        }
        else
        {
            if (isInGame)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
            }
        }
    }

    void OnBackToMenuButtonClicked() {
        SceneManager.LoadScene(0);
    }

    void OnSensitivityChanged(float newValue)
    {
        m_SettingsData.mouseSensitivity = newValue;
    }

    void OnFovChanged(float newValue)
    {
        m_SettingsData.fov = newValue;
    }

    void OnSFXChanged(float newValue)
    {
        m_SettingsData.sfxVolume = newValue;
    }
    void OnMusicChanged(float newValue)
    {
        m_SettingsData.musicVolume = newValue;
    }
    void OnDebugChanged(bool newValue)
    {
        m_SettingsData.debugEnabled = newValue;
    }
    void OnControlsClicked()
    {
        controlRoot.SetActive(true);
        EventSystem.current.SetSelectedGameObject(sensitivitySlider.gameObject);
    }

    void OnBackButtonClicked()
    {
        controlRoot.SetActive(false);
    }

    void OnReturnToGameButtonClicked()
    {
        SetPauseMenuActivation(false);
    }
    void OnSprintToggle(bool newValue)
    {
        m_SettingsData.toggleSprint = newValue;
    }
    void OnCrouchToggle(bool newValue)
    {
        m_SettingsData.toggleCrouch = newValue;
    }
    void OnInteractToggle(bool newValue)
    {
        m_SettingsData.toggleInteract = newValue;
    }

}
