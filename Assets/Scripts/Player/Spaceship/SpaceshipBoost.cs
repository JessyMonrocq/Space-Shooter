using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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

    private void OnBoostStarted(InputAction.CallbackContext context)
    {
        if (canBoost && !isBoosting)
        {
            // Boost
            isBoosting = true;
        }
        else if (canBoost && isBoosting)
        {
            // Interrupt Boost
            isBoosting = false;
        }

        OnSpaceshipBoost?.Invoke(isBoosting);
    }
}
