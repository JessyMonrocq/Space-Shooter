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

    private float boostThrustMultiplier = 2.5f;
    private float boostEnergyCapacity = 150f;
    private float boostEnergyConsumption = 15f;
    private float boostCooldownDuration = 1f;
    private float boostRechargeRate = 1f;

    private float dodgeEnergyConsumption = 25f;
    private float dodgeCooldownDuration = 1f;

    public float CurrentBoostEnergy { get { return currentBoostEnergy; } }
    public float EnergyCapacity { get { return boostEnergyCapacity; } }

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

    #region Public Methods
    public void InitializeBoostValues(SpaceshipStatsSO spaceshipStats)
    {
        boostThrustMultiplier = spaceshipStats.boostThrustMultiplier;
        boostEnergyCapacity = spaceshipStats.boostEnergyCapacity;
        boostEnergyConsumption = spaceshipStats.boostEnergyConsumption;
        boostCooldownDuration = spaceshipStats.boostCooldownDuration;
        boostRechargeRate = spaceshipStats.boostRechargeRate;

        dodgeEnergyConsumption = spaceshipStats.dodgeEnergyConsumption;
        dodgeCooldownDuration = spaceshipStats.dodgeCooldownDuration;
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