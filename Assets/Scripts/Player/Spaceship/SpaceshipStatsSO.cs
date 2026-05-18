using Unity.Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "SpaceshipStatsSO", menuName = "Scriptable Objects/SpaceshipStatsSO")]
public class SpaceshipStatsSO : ScriptableObject
{
    [Header("Speed Settings")]
    [Tooltip("Thrust : defines the spaceship max speed")]
    public float thrust = 250f;
    [Tooltip("Horizontal Thrust : defines the spaceship horizontal speed (left/right movement)")]
    public float horizontalThrust = 50f;
    [Tooltip("Vertical Trhust : defines the spaceship vertical speed (up/down movement)")]
    public float verticalThrust = 50f;
    [Tooltip("Yaw Torque : defines the spaceship yaw rotation speed (left/right turning)")]
    public float yawTorque = 5f;
    [Tooltip("Pitch Torque : defines the spaceship pitch rotation speed (up/down turning)")]
    public float pitchTorque = 5f;
    [Tooltip("Roll Torque : defines the spaceship roll rotation speed (spinning rotation)")]
    public float rollTorque = 5f;
    [Tooltip("Backward Thrust Reduction : defines how much the backward thrust speed is reduced (lower values = slower speed)")]
    [Range(0.001f, 0.999f)] public float backwardThrustReduction = 0.5f;

    [Header("Glide Settings")]
    [Tooltip("Thrust Glide Reduction : defines the spaceship gliding amount when thrust has stopped (higher values = more gliding)")]
    [Range(0.001f, 0.999f)] public float thrustGlideReduction = 0.9f;
    [Tooltip("Horizontal Glide Reduction : defines the spaceship gliding amount when horizontal thrust has stopped (higher values => more gliding)")]
    [Range(0.001f, 0.999f)] public float horizontalGlideReduction = 0.1f;
    [Tooltip("Vertical Glide Reduction : defines the spaceship gliding amount when vertical thrust has stopped (higher values => more gliding)")]
    [Range(0.001f, 0.999f)] public float verticalGlideReduction = 0.1f;

    [Header("Inertia Dampener Settings")]
    [Tooltip("Inertia Translation Dampener Multiplier : defines the impact of spaceship thrust on horizontal/vertical thrust (higher values => slower movement at high speed)")]
    [Range(0f, 2f)] public float inertiaTranslationDampenerMultiplier = 1f;
    [Tooltip("Inertia Torque Dampener Multiplier : defines the impact of spaceship thrust on turning speed (higher values => slower turning at high speed)")]
    [Range(0f, 2f)] public float inertiaTorqueDampenerMultiplier = 1f;
    [Tooltip("Inertia Roll Dampener Multiplier : defines the impact of spaceship thrust on spinning speed (higher values => slower spinning at high speed)")]
    [Range(0f, 1f)] public float inertiaRollDampenerMultiplier = 0.5f;
    [Tooltip("Inertia Recovery Speed : defines the speed at which the inertia dampener values revert back to default")]
    [Range(0.001f, 0.999f)] public float inertiaRecoverySpeed = 0.05f;

    [Header("Flight Assist Settings")]
    [Tooltip("Flight Assist Strength : reduces the amount of momentum kept when moving forward and turning (higher value => tighter turning/less drifting)")]
    public float flightAssistStrength = 2f;

    [Space(30)]
    [Header("Boost Settings")]
    [Tooltip("Boost Thrust Multiplier : defines the boost speed gain (when boost activated => Speed = Thrust x BTM)")]
    public float boostThrustMultiplier = 2.5f;
    [Tooltip("Boost Energy Capacity : defines the maximum capacity of the boost energy")]
    public float boostEnergyCapacity = 150f;
    [Tooltip("Boost Energy Consumption : defines the rate at which boost energy is consumed (energy/seconds)")]
    public float boostEnergyConsumption = 15f;
    [Tooltip("Boost Cooldown Duration : defines how long until the boost can recharge (seconds)")]
    public float boostCooldownDuration = 1f;
    [Tooltip("Boost Recharge Rate : defines the recharging speed of the boost duration (energy/seconds)")]
    public float boostRechargeRate = 1f;
    [Tooltip("Boost Inertia Mulitplier : defines the impact of the boost on the spaceship's inertia settings")]
    public float boostInertiaMultiplier = 1f;

    [Header("Dodge Settings")]
    [Tooltip("Dodge Thrust : defines the speed of the dodge movement")]
    public float dodgeThrust = 50f;
    [Tooltip("Dodge Duration : defines the duration of the dodge movement (seconds)")]
    public float dodgeDuration = 0.25f;
    [Tooltip("Dodge Energy Consumption : defines the amount of energy consumed by the dodge")]
    public float dodgeEnergyConsumption = 25f;
    [Tooltip("Dodge Cooldown Duration : defines how long until the dodge can be performed again (seconds)")]
    public float dodgeCooldownDuration = 1f;

    [Space(30)]
    [Header("Health Settings")]
    [Tooltip("Shield Max Capacity : defines the shield max capacity/energy")]
    public int shieldMaxCapacity = 50;
    [Tooltip("Shield Cooldown : defines how long until the shield can recharge (seconds)")]
    public float shieldCooldown = 1f;
    [Tooltip("Shield Recharge Rate : defines the recharging speed of the shield capacity/energy (energy/seconds)")]
    public float shieldRechargeRate = 1f;
    [Tooltip("Hull Max HP : defines the spaceship's hull maximum Health Points (HP)")]
    public int hullMaxHP = 100;

    [Header("Damage Settings")]
    [Tooltip("Collision Damage To Shield Multiplier : defines the damage dealt to the shield from colliding into an object")]
    public float collisionDamageToShieldMultiplier = 0.33f;
    [Tooltip("Collision Damage To Hull Multiplier : defines the damage dealt to the hull from colliding into an object")]
    public float collisionDamageToHullMultiplier = 0.5f;
    [Tooltip("Collision Cooldown Duration : defines the delay until damage from colliding is registered again (not gameplay relevant)")]
    public float collisionCooldownDuration = 1f;

    [Space(30)]
    [Header("Camera Settings")]
    [Tooltip("Camera Position Offset : defines the position offset of the camera relative to the spaceship")]
    public Vector3 cameraPositionOffset;
    [Tooltip("Camera Position Damp Time : defines the camera position dampening time (follow intensity)")]
    public float cameraPositionDampTime = 0.1f;
    [Tooltip("Camera Rotation Damp Time : defines the camera rotation dampening smoothness (follow intensity)")]
    public float cameraRotationDampSmoothness = 8f;

    [Tooltip("Default Noise Settings : defines the noise settings for the camera default state (flight)")]
    public NoiseSettings defaultNoiseSettings;
    [Tooltip("Boost Noise Settings : defines the noise settings for the camera boost state")]
    public NoiseSettings boostNoiseSettings;
    [Tooltip("Default Noise Amplitude : defines the noise amplitude (strength) for the camera default state")]
    public float defaultNoiseAmplitude = 0.5f;
    [Tooltip("Default Noise Frequency : defines the noise frequency (speed) for the camera default state")]
    public float defaultNoiseFrequency = 0.5f;
    [Tooltip("Boost Noise Amplitude : defines the noise amplitude (strength) for the camera boost state")]
    public float boostNoiseAmplitude = 1.0f;
    [Tooltip("Boost Noise Frequency : defines the noise frequency (speed) for the camera boost state")]
    public float boostNoiseFrequency = 1.0f;

    [Space(30)]
    [Header("VFX Settings")]
    [Range(0.001f, 0.999f)] public float minSpeedThreshold = 0.1f;

    [Space(30)]
    [Header("Cargo Settings")]
    [Tooltip("Max Cargo Space : defines the spaceship maximum cargo space")]
    public int maxCargoSpace;

    //...
}
