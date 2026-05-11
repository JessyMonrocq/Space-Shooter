using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// This script handles the spaceship boost mechanic, increasing thrust and inertia while moving forward automatically
/// As well as the dodge mechanic, using boost energy to quickly shifts in the translation direction
/// </summary>
public class SpaceshipBoost : MonoBehaviour
{
    #region Inspector Fields
    public event Action<bool> OnSpaceshipBoost;
    public event Action OnSpaceshipDodge;
    public event Action<float, float> InitializeBoost;
    public event Action<float, float> InitializeDodge;

    [Header("Boost Settings")]
    [SerializeField] private float boostThrustMultiplier = 2.5f;
    [SerializeField] private float boostEnergyCapacity = 150f;
    [SerializeField] private float boostEnergyConsumption = 15f;
    [SerializeField] private float boostCooldownDuration = 1f;
    [SerializeField] private float boostRechargeRate = 1f;
    [SerializeField, Range(0.001f, 0.999f)] private float boostInertiaMultiplier = 1f;

    [Header("Dodge Settings")]
    [SerializeField] private float dodgeThrust = 50f;
    [SerializeField] private float dodgeDuration = 0.25f;
    [SerializeField] private float dodgeEnergyConsumption = 25f;
    [SerializeField] private float dodgeCooldownDuration = 1f;

    private float currentBoostEnergy;
    private float boostCooldownTimer;
    private float dodgeCooldownTimer;

    private bool isBoosting;
    private bool canBoost;
    private bool canDodge;
    #endregion

    #region Unity Methods
    private void Start()
    {
        currentBoostEnergy = boostEnergyCapacity;
        boostCooldownTimer = 0f;
        dodgeCooldownTimer = 0f;

        isBoosting = false;
        canBoost = true;
        canDodge = true;

        InputManager.Instance.SpaceshipBoost.started += OnBoostStarted;
        InputManager.Instance.SpaceshipDodge.started += OnDodgeStarted;

        InitializeBoost?.Invoke(boostThrustMultiplier, boostInertiaMultiplier);
        InitializeDodge?.Invoke(dodgeThrust, dodgeDuration);
    }

    private void OnDestroy()
    {
        InputManager.Instance.SpaceshipBoost.started -= OnBoostStarted;
        InputManager.Instance.SpaceshipDodge.started -= OnDodgeStarted;
    }

    private void Update()
    {
        HandleBoostEnergy();
        HandleDodgeCooldown();
    }
    #endregion

    #region Private Methods
    private void OnBoostStarted(InputAction.CallbackContext context)
    {
        if (canBoost && !isBoosting)
        {
            isBoosting = true;
        }
        else if (canBoost && isBoosting)
        {
            canBoost = false;
            boostCooldownTimer = 0f;
            isBoosting = false;
        }

        OnSpaceshipBoost?.Invoke(isBoosting);
    }

    private void OnDodgeStarted(InputAction.CallbackContext context)
    {
        if (!canDodge)
        {
            return;
        }

        if (currentBoostEnergy < dodgeEnergyConsumption)
        {
            return;
        }


        canDodge = false;
        currentBoostEnergy -= dodgeEnergyConsumption;
        OnSpaceshipDodge.Invoke();
    }

    private void HandleBoostEnergy()
    {
        if (isBoosting)
        {
            currentBoostEnergy -= boostEnergyConsumption * Time.deltaTime;
            if (currentBoostEnergy <= 0f)
            {
                isBoosting = false;
                canBoost = false;
                boostCooldownTimer = 0f;
                currentBoostEnergy = 0f;
                OnSpaceshipBoost?.Invoke(isBoosting);
            }
            return;
        }

        if (!canBoost)
        {
            boostCooldownTimer += Time.deltaTime;
            if (boostCooldownTimer >= boostCooldownDuration)
            {
                boostCooldownTimer = 0f;
                canBoost = true;
            }
            return;
        }

        if (currentBoostEnergy < boostEnergyCapacity)
        {
            currentBoostEnergy += boostRechargeRate * Time.deltaTime;
            currentBoostEnergy = Mathf.Min(currentBoostEnergy, boostEnergyCapacity);
        }
    }

    private void HandleDodgeCooldown()
    {
        if (!canDodge)
        {
            dodgeCooldownTimer += Time.deltaTime;
            if (dodgeCooldownTimer >= dodgeCooldownDuration)
            {
                dodgeCooldownTimer = 0f;
                canDodge = true;
            }
        }
    }
    #endregion
}