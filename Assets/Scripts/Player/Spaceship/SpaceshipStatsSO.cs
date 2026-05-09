using UnityEngine;

[CreateAssetMenu(fileName = "SpaceshipStatsSO", menuName = "Scriptable Objects/SpaceshipStatsSO")]
public class SpaceshipStatsSO : ScriptableObject
{
    [Header("Movement Settings")]
    [Space(10)]
    [Header("Speed Settings")]
    [Tooltip("Thrust : defines the spaceship max speed")]
    [SerializeField] private float thrust = 250f;
    [Tooltip("Horizontal Thrust : defines the spaceship horizontal speed (left/right movement)")]
    [SerializeField] private float horizontalThrust = 50f;
    [Tooltip("Vertical Trhust : defines the spaceship vertical speed (up/down movement)")]
    [SerializeField] private float verticalThrust = 50f;
    [Tooltip("Yaw Torque : defines the spaceship yaw rotation speed (left/right turning)")]
    [SerializeField] private float yawTorque = 5f;
    [Tooltip("Pitch Torque : defines the spaceship pitch rotation speed (up/down turning)")]
    [SerializeField] private float pitchTorque = 5f;
    [Tooltip("Roll Torque : defines the spaceship roll rotation speed (spinning rotation)")]
    [SerializeField] private float rollTorque = 5f;

    [Header("Glide Settings")]
    [Tooltip("Thrust Glide Reduction : defines the spaceship gliding amount when thrust has stopped (higher values = more gliding)")]
    [SerializeField, Range(0.001f, 0.999f)] private float thrustGlideReduction = 0.9f;
    [Tooltip("Horizontal Glide Reduction : defines the spaceship gliding amount when horizontal thrust has stopped (higher values => more gliding)")]
    [SerializeField, Range(0.001f, 0.999f)] private float horizontalGlideReduction = 0.1f;
    [Tooltip("Vertical Glide Reduction : defines the spaceship gliding amount when vertical thrust has stopped (higher values => more gliding)")]
    [SerializeField, Range(0.001f, 0.999f)] private float verticalGlideReduction = 0.1f;

    [Header("Inertia Dampener Settings")]
    [Tooltip("Inertia Translation Dampener Multiplier : defines the impact of spaceship thrust on horizontal/vertical thrust (higher values => slower movement at high speed)")]
    [SerializeField, Range(0f, 2f)] private float inertiaTranslationDampenerMultiplier = 1f;
    [Tooltip("Inertia Torque Dampener Multiplier : defines the impact of spaceship thrust on turning speed (higher values => slower turning at high speed)")]
    [SerializeField, Range(0f, 2f)] private float inertiaTorqueDampenerMultiplier = 1f;
    [Tooltip("Inertia Roll Dampener Multiplier : defines the impact of spaceship thrust on spinning speed (higher values => slower spinning at high speed)")]
    [SerializeField, Range(0f, 1f)] private float inertiaRollDampenerMultiplier = 0.5f;
    [Tooltip("Inertia Recovery Speed : defines the speed at which the inertia dampener values revert back to default")]
    [SerializeField, Range(0.001f, 1f)] private float inertiaRecoverySpeed = 0.05f;

    [Header("Flight Assist Settings")]
    [Tooltip("Flight Assist Strength : reduces the amount of momentum kept when moving forward and turning (higher value => tighter turning/less drifting)")]
    [SerializeField] float flightAssistStrength = 2f;

    [Space(30)]
    [Header("Boost Settings")]
    [Tooltip("Boost Thrust Multiplier : defines the boost speed gain (when boost activated => Speed = Thrust x BTM)")]
    [SerializeField] private float boostThrustMultiplier = 2.5f;
    [Tooltip("Boost Duration : defines the maximum capacity of the boost energy")]
    [SerializeField] private float boostEnergyCapacity = 150f;
    [Tooltip("Boost Duration : defines the rate at which boost energy is consumed (energy/seconds)")]
    [SerializeField] private float boostEnergyConsumption = 15f;
    [Tooltip("Boost Cooldown Duration : defines how long until the boost can recharge (seconds)")]
    [SerializeField] private float boostCooldownDuration = 1f;
    [Tooltip("Boost Recharge Rate : defines the recharging speed of the boost duration (energy/seconds)")]
    [SerializeField] private float boostRechargeRate = 1f;
    [Tooltip("Boost Inertia Mulitplier : defines the impact of the boost on the spaceship's inertia settings")]
    [SerializeField] private float boostInertiaMultiplier = 1f;

    [Space(30)]
    [Header("Weapons Settings")]
    [Tooltip("Weapons Fire Rate : defines the weapons rate of fire (projectiles/seconds)")]
    [SerializeField] private float weaponsFireRate = 1f;
    [Tooltip("Weapons Damage : defines the damage of a single projectile")]
    [SerializeField] private int weaponsDamage = 1;
    [Tooltip("Weapons Max Ammo : defines the weapons ammunitions max capacity")]
    [SerializeField] private int weaponsMaxAmmo = 100;
    [Tooltip("Weapons Cooldown Duration : defines how long until the weapon can reload (seconds)")]
    [SerializeField] private float weaponsCooldownDuration = 1f;
    [Tooltip("Weapons Reload Speed : defines the reloading speed of the weapons (ammunition/seconds)")]
    [SerializeField] private float weaponsReloadSpeed = 1f;

    [Space(30)]
    [Header("Health Settings")]
    [Tooltip("Shield Max Capacity : defines the shield max capacity/energy")]
    [SerializeField] private int shieldMaxCapacity = 50;
    [Tooltip("Shield Cooldown : defines how long until the shield can recharge (seconds)")]
    [SerializeField] private float shieldCooldown = 1f;
    [Tooltip("Shield Recharge Rate : defines the recharging speed of the shield capacity/energy (energy/seconds)")]
    [SerializeField] private float shieldRechargeRate = 1f;
    [Tooltip("Hull Max HP : defines the spaceship's hull maximum Health Points (HP)")]
    [SerializeField] private int hullMaxHP = 100;

    [Space(30)]
    [Header("Camera Settings")]
    [Tooltip("Camera Distance : defines the distance at which the camera is positionned behind the spaceship at rest")]
    [SerializeField] private float cameraDistance = 30f;
    [Tooltip("Camera Position Dampening : defines the camera position dampening amount (follow intensity)")]
    [SerializeField] private Vector3 cameraPositionDampening = Vector3.zero;
    [Tooltip("Camera Rotation Dampening : defines the camera rotation dampening amount (follow intensity)")]
    [SerializeField] private Vector3 cameraRotationDampening = Vector3.zero;

    [Space(30)]
    [Header("Cargo Settings")]
    [Tooltip("Max Cargo Space : defines the spaceship maximum cargo space")]
    [SerializeField] private int maxCargoSpace;

    //...
}
