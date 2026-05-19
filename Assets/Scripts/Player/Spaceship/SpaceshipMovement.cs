using UnityEngine;

/// <summary>
/// This script handles the movement of the spaceship, with thrust, horizontal and vertical movement, and rotation (pitch, yaw, roll).
/// It also implements multiple inertia dampeners that affect the ship horizontal/vertical movement and rotation based on thrust speed.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SpaceshipMovement : MonoBehaviour
{
    #region Inspector Fields
    private float thrust = 250f;
    private float horizontalThrust = 50f;
    private float verticalThrust = 50f;
    private float yawTorque = 5f;
    private float pitchTorque = 5f;
    private float rollTorque = 5f;
    private float backwardThrustReduction = 0.5f;

    private float thrustGlideReduction = 0.9f;
    private float horizontalGlideReduction = 0.111f;
    private float verticalGlideReduction = 0.111f;

    private float inertiaTranslationDampenerMultiplier = 1f;
    private float inertiaTorqueDampenerMultiplier = 1f;
    private float inertiaRollDampenerMultiplier = 0.5f;
    private float inertiaRecoverySpeed = 0.05f;

    float flightAssistStrength = 2f;

    public enum MovementState
    {
        FlightMode,
        BoostMode,
        FightMode
    }

    public float CurrentSpeed { get { return currentSpeed; } }

    private MovementState movementState;

    private Rigidbody spaceshipRB;

    private float thrustInput;
    private float horizontalThrustInput;
    private float verticalThrustInput;
    private float pitchInput;
    private float yawInput;
    private float rollInput;

    private float currentSpeed;

    private float glide;
    private float horizontalGlide;
    private float verticalGlide;

    private float inertiaTorqueDampener;
    private float inertiaTranslationDampener;
    private float inertiaRollDampener;
    private float targetInertiaTorqueDampener;
    private float targetInertiaTranslationDampener;
    private float targetRollDampener;

    private float boostThrustMultiplier;
    private float boostInertiaMultiplier;
    private float boostInertia;

    private Vector3 dodgeDirection;
    private float dodgeThrust;
    private float dodgeDuration;
    private float dodgeTimer;

    private bool isDodging;
    #endregion

    #region Unity Methods
    private void Start()
    {
        spaceshipRB = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        InitializeParameters();
    }

    private void Update()
    {
        ReadInputValues();

        currentSpeed = Vector3.Dot(transform.forward, spaceshipRB.linearVelocity);
    }

    private void FixedUpdate()
    {
        UpdateInertiaDampeners();
        ApplyTranslationForces();
        ApplyRotationForces();

        if (isDodging)
        {
            HandleDodging();
        }
        else
        {
            ApplyFlightAssistMethod();
        }
    }
    #endregion

    #region Public Methods
    public void InitializeMovementValues(SpaceshipStatsSO spaceshipStats)
    {
        thrust = spaceshipStats.thrust;
        horizontalThrust = spaceshipStats.horizontalThrust;
        verticalThrust = spaceshipStats.verticalThrust;
        yawTorque = spaceshipStats.yawTorque;
        pitchTorque = spaceshipStats.pitchTorque;
        rollTorque = spaceshipStats.rollTorque;
        backwardThrustReduction = spaceshipStats.backwardThrustReduction;
        thrustGlideReduction = spaceshipStats.thrustGlideReduction;
        horizontalGlideReduction = spaceshipStats.horizontalGlideReduction;
        verticalGlideReduction = spaceshipStats.verticalGlideReduction;
        inertiaTranslationDampenerMultiplier = spaceshipStats.inertiaTranslationDampenerMultiplier;
        inertiaTorqueDampenerMultiplier = spaceshipStats.inertiaTorqueDampenerMultiplier;
        inertiaRollDampenerMultiplier = spaceshipStats.inertiaRollDampenerMultiplier;
        inertiaRecoverySpeed = spaceshipStats.inertiaRecoverySpeed;
        flightAssistStrength = spaceshipStats.flightAssistStrength;

        boostThrustMultiplier = spaceshipStats.boostThrustMultiplier;
        boostInertiaMultiplier = spaceshipStats.boostInertiaMultiplier;

        dodgeThrust = spaceshipStats.dodgeThrust;
        dodgeDuration = spaceshipStats.dodgeDuration;
    }

    public void SetFightMode(bool state)
    {
        if (movementState == MovementState.BoostMode && state)
        {
            SetBoostMode(false);
            movementState = MovementState.FightMode;
            glide = 0f;
            return;
        }

        if (movementState == MovementState.FlightMode && state)
        {
            movementState = MovementState.FightMode;
            glide = 0f;
            return;
        }

        if (!state)
        {
            movementState = MovementState.FlightMode;
        }
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

    public void Dodge()
    {
        isDodging = true;
        dodgeTimer = 0f;
        Vector3 direction = (horizontalThrustInput * Vector3.right + verticalThrustInput * Vector3.up).normalized;

        if (direction.sqrMagnitude >= 0.01f)
        {
            dodgeDirection = direction;
        }
    }
    #endregion

    #region Private Methods
    private void InitializeParameters()
    {
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
        targetInertiaTorqueDampener = 1f;
        targetInertiaTranslationDampener = 1f;
        targetRollDampener = 1f;

        boostInertia = 1f;

        dodgeDirection = Vector3.right;
        dodgeTimer = 0f;
        isDodging = false;

        movementState = MovementState.FlightMode;
    }

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
        if (Mathf.Approximately(thrustInput, 0f) && movementState == MovementState.FlightMode)
        {
            currentThrust = glide;
        }
        else
        {
            bool isFlightMode = movementState == MovementState.FlightMode;
            currentThrust = thrust * (isFlightMode ? Mathf.Abs(thrustInput) : 1f);
        }

        targetInertiaTorqueDampener = 1f / (1f + (currentThrust * inertiaTorqueDampenerMultiplier / 100f));
        targetInertiaTranslationDampener = 1f / (1f + (currentThrust * inertiaTranslationDampenerMultiplier / 100f));
        targetRollDampener = 1f / (1f + (currentThrust * inertiaRollDampenerMultiplier / 100f));

        inertiaTorqueDampener = Mathf.MoveTowards(inertiaTorqueDampener, targetInertiaTorqueDampener, inertiaRecoverySpeed);
        inertiaTranslationDampener = Mathf.MoveTowards(inertiaTranslationDampener, targetInertiaTranslationDampener, inertiaRecoverySpeed);
        inertiaRollDampener = Mathf.MoveTowards(inertiaRollDampener, targetRollDampener, inertiaRecoverySpeed);
    }

    private void ApplyTranslationForces()
    {
        // Forward Thrust
        if (movementState == MovementState.FlightMode)
        {
            float thrustInputClamp = Mathf.Clamp(thrustInput, -backwardThrustReduction, 1f);
            if (Mathf.Approximately(thrustInput, 0f))
            {
                spaceshipRB.AddRelativeForce(Vector3.forward * glide * Time.fixedDeltaTime, ForceMode.VelocityChange);
                glide *= thrustGlideReduction;
            }
            else
            {
                spaceshipRB.AddRelativeForce(Vector3.forward * thrustInputClamp * thrust * Time.fixedDeltaTime, ForceMode.VelocityChange);
                glide = thrust;
            }
        }
        else if (movementState == MovementState.BoostMode)
        {
            {
                spaceshipRB.AddRelativeForce(Vector3.forward * boostThrustMultiplier * thrust * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }

        if (!isDodging)
        {
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

    private void HandleDodging()
    {
        dodgeTimer += Time.fixedDeltaTime;
        float thrustMultiplier = Mathf.Max(horizontalThrust, verticalThrust);
        spaceshipRB.AddRelativeForce(dodgeDirection * dodgeThrust * thrustMultiplier, ForceMode.Impulse);
        if (dodgeTimer >= dodgeDuration)
        {
            dodgeTimer = 0f;
            isDodging = false;
        }
    }
    #endregion
}