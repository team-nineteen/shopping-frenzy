using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    
    [Tooltip("Additional sensitivity multiplier for WebGL")]
    public float webglLookSensitivityMultiplier = 0.25f;
    [Tooltip("Limit to consider an input when using a trigger on a controller")]
    public bool invertYAxis = false;

    public bool isInteractToggled {get; set;} = false;
    public bool isSprintToggled {get; set;} = false;
    public bool isCrouchToggled {get; set;} = false;
    GameFlowManager m_GameFlowManager;
    SettingsData m_SettingsData;
    public MenuManager m_PauseMenu;
    PlayerCharacterController m_PlayerCharacterController;
    bool m_InteractInputWasHeld;

    private void Start()
    {
        m_SettingsData = FindObjectOfType<SettingsData>();
        DebugUtility.HandleErrorIfNullFindObject<SettingsData, MenuManager>(m_SettingsData, this);
        m_PlayerCharacterController = GetComponent<PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, PlayerInputHandler>(m_PlayerCharacterController, this, gameObject);
        m_GameFlowManager = FindObjectOfType<GameFlowManager>();
        DebugUtility.HandleErrorIfNullFindObject<GameFlowManager, PlayerInputHandler>(m_GameFlowManager, this);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (m_SettingsData.toggleInteract && Input.GetButtonDown(GameConstants.k_ButtonNameInteract))
            isInteractToggled = !isInteractToggled;
        if (m_SettingsData.toggleSprint && Input.GetButtonDown(GameConstants.k_ButtonNameSprint))
            isSprintToggled = !isSprintToggled;
        if (m_SettingsData.toggleCrouch && Input.GetButtonDown(GameConstants.k_ButtonNameCrouch))
            isCrouchToggled = !isCrouchToggled;
        if (!m_PauseMenu.gameObject.activeSelf && Input.GetButtonDown(GameConstants.k_ButtonNamePauseMenu))
            m_PauseMenu.SetPauseMenuActivation(true);
    }

    private void LateUpdate()
    {
        m_InteractInputWasHeld = GetInteractInputHeld();
    }

    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked && !m_GameFlowManager.gameIsEnding;
    }

    public Vector3 GetMoveInput()
    {
        if (CanProcessInput())
        {
            Vector3 move = new Vector3(Input.GetAxisRaw(GameConstants.k_AxisNameHorizontal), 0f, Input.GetAxisRaw(GameConstants.k_AxisNameVertical));

            // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
            move = Vector3.ClampMagnitude(move, 1);

            return move;
        }

        return Vector3.zero;
    }

    public float GetLookInputsHorizontal()
    {
        return GetMouseLookAxis(GameConstants.k_MouseAxisNameHorizontal);
    }

    public float GetLookInputsVertical()
    {
        return GetMouseLookAxis(GameConstants.k_MouseAxisNameVertical);
    }

    public bool GetJumpInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown(GameConstants.k_ButtonNameJump);
        }

        return false;
    }

    public bool GetJumpInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetButton(GameConstants.k_ButtonNameJump);
        }

        return false;
    }

    public bool GetInteractInputDown()
    {
        return GetInteractInputHeld() && !m_InteractInputWasHeld;
    }

    public bool GetInteractInputReleased()
    {
        return !GetInteractInputHeld() && m_InteractInputWasHeld;
    }

    public bool GetInteractInputHeld()
    {
        if (CanProcessInput())
        {
            return m_SettingsData.toggleInteract ? isInteractToggled : Input.GetButton(GameConstants.k_ButtonNameInteract);
        }

        return false;
    }

    public bool GetAimInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetButton(GameConstants.k_ButtonNameAlternateInteract);
        }

        return false;
    }

    public bool GetSprintInputHeld()
    {
        if (CanProcessInput())
        {
            return m_SettingsData.toggleSprint ? isSprintToggled : Input.GetButton(GameConstants.k_ButtonNameSprint);
        }

        return false;
    }

    public bool GetCrouchInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown(GameConstants.k_ButtonNameCrouch);
        }

        return false;
    }

    public bool GetCrouchInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetButton(GameConstants.k_ButtonNameCrouch);
        }

        return false;
    }

    float GetMouseLookAxis(string mouseInputName)
    {
        if (CanProcessInput())
        {
            // Get look input coming from the mouse
            float i = Input.GetAxisRaw(mouseInputName);

            // handle inverting vertical input
            if (!invertYAxis)
                i *= -1f;

            // apply sensitivity multiplier
            i *= m_SettingsData.mouseSensitivity * 0.01f;

#if UNITY_WEBGL
            // Mouse tends to be even more sensitive in WebGL due to mouse acceleration, so reduce it even more
            i *= webglLookSensitivityMultiplier;
#endif
            return i;
        }

        return 0f;
    }
}
