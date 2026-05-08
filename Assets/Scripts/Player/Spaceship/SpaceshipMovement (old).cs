using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Obsolete("First prototype version of the spaceship movement")]
[RequireComponent(typeof(Rigidbody))]
public class SpaceshipMovementV1 : MonoBehaviour
{
    #region Inspector Fields
    private Rigidbody spaceshipRigidbody;

    public float CurrentForwardSpeed => currentForwardSpeed;

    [Header("Movement Settings")]
    [Header("Forward Movement")]
    [SerializeField] private float forwardMaxSpeed = 100f;
    [Tooltip("Acceleration : Time in seconds for the spaceship to reach max forward speed")]
    [SerializeField] private float forwardAccelerationTime = 1f;
    [Tooltip("Deceleration : Time in seconds for the spaceship to slow down back to zero")]
    [SerializeField] private float forwardDecelerationTime = 1f;
    [SerializeField] private float brakingDecelerationMultiplier = 2f;

    [Header("Translation Movement")]
    [SerializeField] private float translationMaxSpeed = 5f;
    [SerializeField] private float translationAccelerationTime = 1f;

    [Header("Rotation Movement")]
    [SerializeField] private float rotationMaxSpeed = 0.1f;
    [SerializeField] private float rotationAccelerationTime = 0.05f;

    [Header("Roll Movement")]
    [SerializeField] private float rollMaxSpeed = 0.1f;
    [SerializeField] private float rollAccelerationTime = 0.05f;

    [Header("Inertia Settings")]
    [SerializeField] private float inertiaEffectMultiplier = 1.5f;

    private float currentForwardSpeed;
    private float currentTranslationSpeed;
    private float currentRotationSpeed;
    private float currentRollSpeed;

    private float forwardInertiaRatio;

    private float forwardMoveInput;
    private float horizontalMoveInput;
    private float verticalMoveInput;
    private float pitchInput;
    private float yawInput;
    private float rollInput;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        spaceshipRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        currentForwardSpeed = 0f;
        currentTranslationSpeed = 0f;
        currentRotationSpeed = 0f;
        currentRollSpeed = 0f;

        forwardInertiaRatio = 0f;
    }

    private void OnDisable()
    {

    }
    #endregion

    #region Input Events

    #endregion

    #region Update Method
    private void Update()
    {
        ReadInputValues();
    }

    private void FixedUpdate()
    {
        ApplyForwardMovement();

        float inertiaMultiplier = 1f / (1f + forwardInertiaRatio * inertiaEffectMultiplier);

        spaceshipRigidbody.AddForce(spaceshipRigidbody.transform.TransformDirection(Vector3.right) * horizontalMoveInput * translationMaxSpeed * inertiaMultiplier, ForceMode.VelocityChange);
        spaceshipRigidbody.AddForce(spaceshipRigidbody.transform.TransformDirection(Vector3.up) * verticalMoveInput * translationMaxSpeed * inertiaMultiplier, ForceMode.VelocityChange);

        spaceshipRigidbody.AddTorque(spaceshipRigidbody.transform.right * rotationMaxSpeed * pitchInput * inertiaMultiplier, ForceMode.VelocityChange);
        spaceshipRigidbody.AddTorque(spaceshipRigidbody.transform.up * rotationMaxSpeed * yawInput * inertiaMultiplier, ForceMode.VelocityChange);

        spaceshipRigidbody.AddTorque(spaceshipRigidbody.transform.forward * rollMaxSpeed * rollInput * inertiaMultiplier, ForceMode.VelocityChange);
    }
    #endregion

    #region Private Methods
    private void ReadInputValues()
    {
        forwardMoveInput = InputManager.Instance.SpaceshipForwardMove.ReadValue<float>();
        horizontalMoveInput = InputManager.Instance.SpaceshipHorizontalMove.ReadValue<float>();
        verticalMoveInput = InputManager.Instance.SpaceshipVerticalMove.ReadValue<float>();
        pitchInput = InputManager.Instance.SpaceshipPitch.ReadValue<float>();
        yawInput = InputManager.Instance.SpaceshipYaw.ReadValue<float>();
        rollInput = InputManager.Instance.SpaceshipRoll.ReadValue<float>();
    }

    private void ApplyForwardMovement()
    {
        bool isAccelerating = false;
        bool isBraking = false;
        float targetSpeed = forwardMoveInput * forwardMaxSpeed;
        if (Mathf.Approximately(targetSpeed, 0f))
        {
            isAccelerating = false;
            isBraking = false;
        }
        else if ((targetSpeed > 0 && currentForwardSpeed < 0) || (targetSpeed < 0 && currentForwardSpeed > 0))
        {
            isAccelerating = false;
            isBraking = true;
        }
        else
        {
            isAccelerating = true;
            isBraking = false;
        }

        float decelerationTime = isBraking ? forwardDecelerationTime / brakingDecelerationMultiplier : forwardDecelerationTime;
        currentForwardSpeed = Mathf.MoveTowards(currentForwardSpeed, isAccelerating ? targetSpeed : 0f, (forwardMaxSpeed / (isAccelerating ? forwardAccelerationTime : decelerationTime)) * Time.deltaTime);
        spaceshipRigidbody.AddForce(spaceshipRigidbody.transform.TransformDirection(Vector3.forward) * currentForwardSpeed, ForceMode.VelocityChange);
    
        forwardInertiaRatio = Mathf.Abs(currentForwardSpeed) / forwardMaxSpeed;
    }

    private void ApplyTranslationMovement()
    {

    }
    #endregion
}
