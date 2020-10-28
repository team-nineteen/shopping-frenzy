using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public float lookSensitivity;
    public float webglLookSensitivityMultiplier;
    public float rotationSpeed;
    public float maxSpeedOnGround;
    public float speedModifier;
    public float accelerationSpeedInAir = 25f;
    public float maxSpeedInAir = 10f;
    public float sprintSpeedModifier;
	public float jumpForce;
	public float gravityDownForce = 20f;
    public Camera playerCamera;


	private CharacterController controller;
	private Vector3 direction;
    private float m_CameraVerticalAngle;
    private float m_LastTimeJumped = 0f;
    private Vector3 m_GroundNormal;
    private Vector3 m_CharacterVelocity;

    public bool hasJumpedThisFrame { get; private set; }

    public Vector3 characterVelocity { get; set; }

    private float movementSharpnessOnGround = 1;
    
    

	void Start()
	{
		controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}

	void Update()
	{
        handleCameraMovement();
		handleMovementLegacy(); // This code suffers from not rotating with the camera :^)
	}

    void handleMovementLegacy() {
        Vector3 worldspaceMoveInput = transform.TransformVector(GetMoveInput());
            
        Vector3 targetVelocity = worldspaceMoveInput * maxSpeedOnGround * speedModifier;
        characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementSharpnessOnGround * Time.deltaTime);

        if (controller.isGrounded)
        {
            if (GetJumpInputDown())
            {
                // start by canceling out the vertical component of our velocity
                characterVelocity = new Vector3(characterVelocity.x, 0f, characterVelocity.z);

                // then, add the jumpSpeed value upwards
                characterVelocity += Vector3.up * jumpForce;

                // remember last time we jumped because we need to prevent snapping to ground for a short time
                m_LastTimeJumped = Time.time;
                hasJumpedThisFrame = true;
            }
		}
		else
		{
			// Apply Gravity
			direction.y = direction.y + (Physics.gravity.y * gravityDownForce * Time.deltaTime);
		}
		controller.Move(targetVelocity * Time.deltaTime);
    }

    void handleMovement()
    {
        bool isSprinting = GetSprintInputHeld();
        {
            float speedModifier = isSprinting ? sprintSpeedModifier : 1f;

            Vector3 worldspaceMoveInput = transform.TransformVector(GetMoveInput());

            if (controller.isGrounded)
            {
                // calculate the desired velocity from inputs, max speed, and current slope
                Vector3 targetVelocity = worldspaceMoveInput * maxSpeedOnGround * speedModifier;
                
                targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, m_GroundNormal) * targetVelocity.magnitude;

                // smoothly interpolate between our current velocity and the target velocity based on acceleration speed
                characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementSharpnessOnGround * Time.deltaTime);

                // jumping
                if (controller.isGrounded && GetJumpInputDown())
                {
                    // start by canceling out the vertical component of our velocity
                    characterVelocity = new Vector3(characterVelocity.x, 0f, characterVelocity.z);

                    // then, add the jumpSpeed value upwards
                    characterVelocity += Vector3.up * jumpForce;

                    // remember last time we jumped because we need to prevent snapping to ground for a short time
                    m_LastTimeJumped = Time.time;
                    hasJumpedThisFrame = true;

                    // Force grounding to false
                    // controller.isGrounded = false;
                    m_GroundNormal = Vector3.up;
                }
            }
            else
            {
                characterVelocity += worldspaceMoveInput * accelerationSpeedInAir * Time.deltaTime;

                // limit air speed to a maximum, but only horizontally
                float verticalVelocity = characterVelocity.y;
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(characterVelocity, Vector3.up);
                horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeedInAir * speedModifier);
                characterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

                // apply the gravity to the velocity
                characterVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
            }
        }

        controller.Move(characterVelocity * Time.deltaTime);
    }

    void handleCameraMovement()
    {
        transform.Rotate(new Vector3(0f, (GetLookInputsHorizontal() * rotationSpeed), 0f), Space.Self);
        m_CameraVerticalAngle += GetLookInputsVertical() * rotationSpeed;
        m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89f, 89f);
        playerCamera.transform.localEulerAngles = new Vector3(m_CameraVerticalAngle, 0, 0);
    }
    
    public bool GetSprintInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetButton(GameConstants.k_ButtonNameSprint);
        }

        return false;
    }

    public bool GetJumpInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown(GameConstants.k_ButtonNameJump);
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

    public float GetLookInputsHorizontal()
    {
        return GetMouseAxis(GameConstants.k_MouseAxisNameHorizontal, false);
    }

    public float GetLookInputsVertical()
    {
        return GetMouseAxis(GameConstants.k_MouseAxisNameVertical, true);
    }

    public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
    {
        Vector3 directionRight = Vector3.Cross(direction, transform.up);
        return Vector3.Cross(slopeNormal, directionRight).normalized;
    }

    public Vector3 GetMoveInput()
    {
        if (CanProcessInput())
        {
            Vector3 move = new Vector3(Input.GetAxisRaw(GameConstants.k_AxisNameHorizontal), 0f, Input.GetAxisRaw(GameConstants.k_AxisNameVertical));
            return Vector3.ClampMagnitude(move, 1);
        }

        return Vector3.zero;
    }

    float GetMouseAxis(string mouseInputName, bool invert)
    {
        if (CanProcessInput())
        {
            float i = Input.GetAxisRaw(mouseInputName);

            if (invert)
                i *= -1f;

            i *= lookSensitivity;
            i *= 0.01f;
#if UNITY_WEBGL                
            i *= webglLookSensitivityMultiplier;
#endif
            return i;
        }

        return 0f;
    }
    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }
}
