using UnityEngine;

/// <summary>
/// This script handles the movement of the spaceship, with thrust, horizontal and vertical movement, and rotation (pitch, yaw, roll).
/// It also implements multiple inertia dampeners that affect the ship horizontal/vertical movement and rotation based on thrust speed.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SpaceshipMovement : MonoBehaviour
{
    #region Inspector Fields
    [Header("Movement Settings")]
    [Header("Speed Settings")]
    [SerializeField] private float thrust = 250f;
    [SerializeField] private float horizontalThrust = 50f;
    [SerializeField] private float verticalThrust = 50f;
    [SerializeField] private float yawTorque = 5f;
    [SerializeField] private float pitchTorque = 5f;
    [SerializeField] private float rollTorque = 5f;

    [Header("Glide Settings")]
    [SerializeField, Range(0.001f, 0.999f)] private float thrustGlideReduction = 0.9f;
    [SerializeField, Range(0.001f, 0.999f)] private float horizontalGlideReduction = 0.111f;
    [SerializeField, Range(0.001f, 0.999f)] private float verticalGlideReduction = 0.111f;

    [Header("Inertia Dampener Settings")]
    [SerializeField, Range(0f, 2f)] private float inertiaTranslationDampenerMultiplier = 1f;
    [SerializeField, Range(0f, 2f)] private float inertiaTorqueDampenerMultiplier = 1f;
    [SerializeField, Range(0f, 1f)] private float inertiaRollDampenerMultiplier = 0.5f;

    [Header("Flight Assist Settings")]
    [SerializeField] float flightAssistStrength = 2f;

    public enum MovementState
    {
        FlightMode,
        BoostMode
    }

    private MovementState movementState;

    private Rigidbody spaceshipRB;

    private float thrustInput;
    private float horizontalThrustInput;
    private float verticalThrustInput;
    private float pitchInput;
    private float yawInput;
    private float rollInput;

    private float glide;
    private float horizontalGlide;
    private float verticalGlide;

    private float inertiaTorqueDampener;
    private float inertiaTranslationDampener;
    private float inertiaRollDampener;

    private float boostThrustMultiplier;
    private float boostInertiaMultiplier;
    private float boostInertia;
    #endregion

    #region Unity Methods
    private void Start()
    {
        spaceshipRB = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        thrustInput = 0f;
        horizontalThrustInput = 0f;
        verticalThrustInput = 0f;
        pitchInput = 0f;
        yawInput = 0f;
        rollInput = 0f;
        glide = 0f;
        horizontalGlide = 0f;
        verticalGlide = 0f;
        inertiaTorqueDampener = 0f;
        inertiaTranslationDampener = 0f;
        inertiaRollDampener = 0f;

        boostInertia = 1f;

        movementState = MovementState.FlightMode;
    }

    private void Update()
    {
        ReadInputValues();
    }

    private void FixedUpdate()
    {
        UpdateInertiaDampeners();
        ApplyTranslationForces();
        ApplyRotationForces();
        ApplyFlightAssistMethod();
    }
    #endregion

    #region Public Methods
    public void SetBoostValues(float thrustMult, float inertiaMult)
    {
        boostThrustMultiplier = thrustMult;
        boostInertiaMultiplier = inertiaMult;
    }

    public void SetBoostMode(bool state)
    {
        if (movementState == MovementState.FlightMode && state)
        {
            movementState = MovementState.BoostMode;
            boostInertia = boostInertiaMultiplier;
            return;
        }

        if (movementState == MovementState.BoostMode && !state)
        {
            movementState = MovementState.FlightMode;
            boostInertia = 1f;
            return;
        }
    }
    #endregion

    #region Private Methods
    private void ReadInputValues()
    {
        thrustInput = InputManager.Instance.SpaceshipForwardMove.ReadValue<float>();
        horizontalThrustInput = InputManager.Instance.SpaceshipHorizontalMove.ReadValue<float>();
        verticalThrustInput = InputManager.Instance.SpaceshipVerticalMove.ReadValue<float>();
        pitchInput = InputManager.Instance.SpaceshipPitch.ReadValue<float>();
        yawInput = InputManager.Instance.SpaceshipYaw.ReadValue<float>();
        rollInput = InputManager.Instance.SpaceshipRoll.ReadValue<float>();
    }

    private void UpdateInertiaDampeners()
    {
        float currentThrust;
        if (Mathf.Approximately(thrustInput, 0f))
        {
            currentThrust = glide;
        }
        else
        {
            currentThrust = thrust;
        }
        inertiaTorqueDampener = 1f / (1f + (currentThrust * inertiaTorqueDampenerMultiplier / 100f));
        inertiaTranslationDampener = 1f / (1f + (currentThrust * inertiaTranslationDampenerMultiplier / 100f));
        inertiaRollDampener = 1f / (1f + (currentThrust * inertiaRollDampenerMultiplier / 100f));
    }

    private void ApplyTranslationForces()
    {
        // Thrust : FlightMode
        if (movementState == MovementState.FlightMode)
        {
            if (Mathf.Approximately(thrustInput, 0f))
            {
                spaceshipRB.AddRelativeForce(Vector3.forward * glide * Time.fixedDeltaTime, ForceMode.VelocityChange);
                glide *= thrustGlideReduction;
            }
            else
            {
                spaceshipRB.AddRelativeForce(Vector3.forward * thrustInput * thrust * Time.fixedDeltaTime, ForceMode.VelocityChange);
                glide = thrust;
            }
        }
        // Thrust : BoostMode
        else
        {
            spaceshipRB.AddRelativeForce(Vector3.forward * boostThrustMultiplier * thrust * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        // Horizontal Thrust
        float horizontalForce = 0f;
        if (Mathf.Approximately(horizontalThrustInput, 0f))
        {
            horizontalForce = horizontalGlide * inertiaTranslationDampener * boostInertia * Time.fixedDeltaTime;
            horizontalGlide *= horizontalGlideReduction;
        }
        else
        {
            horizontalForce = horizontalThrustInput * horizontalThrust * inertiaTranslationDampener * boostInertia * Time.fixedDeltaTime;
            horizontalGlide = horizontalThrust * horizontalThrustInput;
        }
        spaceshipRB.AddRelativeForce(Vector3.right * horizontalForce);

        // Vertical Thrust
        float verticalForce = 0f;
        if (Mathf.Approximately(verticalThrustInput, 0f))
        {
            verticalForce = verticalGlide * inertiaTranslationDampener * boostInertia * Time.fixedDeltaTime;
            verticalGlide *= verticalGlideReduction;
        }
        else
        {
            verticalForce = verticalThrustInput * verticalThrust * inertiaTranslationDampener * boostInertia * Time.fixedDeltaTime;
            verticalGlide = verticalThrust * verticalThrustInput;
        }
        spaceshipRB.AddRelativeForce(Vector3.up * verticalForce);
    }

    private void ApplyRotationForces()
    {
        // Pitch
        float pitchForce = Mathf.Clamp(pitchInput, -1f, 1f) * pitchTorque * inertiaTorqueDampener * boostInertia * Time.fixedDeltaTime;
        spaceshipRB.AddRelativeTorque(Vector3.right * pitchForce, ForceMode.VelocityChange);
        // Yaw
        float yawForce = Mathf.Clamp(yawInput, -1f, 1f) * yawTorque * inertiaTorqueDampener * boostInertia * Time.fixedDeltaTime;
        spaceshipRB.AddRelativeTorque(Vector3.up * yawForce, ForceMode.VelocityChange);
        // Roll
        float rollForce = Mathf.Clamp(rollInput, -1f, 1f) * rollTorque * inertiaRollDampener * boostInertia * Time.fixedDeltaTime;
        spaceshipRB.AddRelativeTorque(Vector3.back * rollForce, ForceMode.VelocityChange);
    }

    private void ApplyFlightAssistMethod()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(spaceshipRB.linearVelocity);
        localVelocity.x = Mathf.Lerp(localVelocity.x, horizontalThrustInput * horizontalThrust, flightAssistStrength * Time.fixedDeltaTime);
        localVelocity.y = Mathf.Lerp(localVelocity.y, verticalThrustInput * verticalThrust, flightAssistStrength * Time.fixedDeltaTime);
        spaceshipRB.linearVelocity = transform.TransformDirection(localVelocity);
    }
    #endregion
}
