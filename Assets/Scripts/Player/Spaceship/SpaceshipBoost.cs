using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// This script handles the spaceship boost mechanic, increasing thrust and inertia while moving forward automatically
/// </summary>
public class SpaceshipBoost : MonoBehaviour
{
    public UnityEvent<bool> OnSpaceshipBoost;
    public UnityEvent<float, float> OnBoostInitialize;

    [Header("Boost Settings")]
    [SerializeField] private float boostThrustMultiplier = 2.5f;
    [SerializeField] private float boostEnergyCapacity = 150f;
    [SerializeField] private float boostEnergyConsumption = 15f;
    [SerializeField] private float boostCooldownDuration = 1f;
    [SerializeField] private float boostRechargeRate = 1f;
    [SerializeField, Range(0.001f, 0.999f)] private float boostInertiaMultiplier = 1f;

    private float currentBoostEnergy;
    private float boostCooldownTimer;

    private bool isBoosting;
    private bool canBoost;

    private void Start()
    {
        currentBoostEnergy = boostEnergyCapacity;
        boostCooldownTimer = 0f;

        isBoosting = false;
        canBoost = true;

        InputManager.Instance.SpaceshipBoost.started += OnBoostStarted;

        OnBoostInitialize?.Invoke(boostThrustMultiplier, boostInertiaMultiplier);
    }

    private void OnDisable()
    {
        InputManager.Instance.SpaceshipBoost.started -= OnBoostStarted;
    }

    private void Update()
    {
        HandleBoostEnergy();
    }

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
}
