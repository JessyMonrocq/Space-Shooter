using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipHUD : MonoBehaviour
{
    [Header("HUD Settings")]
    [Header("Status Settings")]
    [SerializeField] private CanvasGroup statusGroup;
    [SerializeField] private CustomSlider shieldBar;
    [SerializeField] private CustomSlider healthBar;
    [SerializeField] private CustomSlider energyBar;

    [Header("Ammo Settings")]
    [SerializeField] private CanvasGroup ammoGroup;
    [SerializeField] private CircleSlider primaryAmmo;
    [SerializeField] private CircleSlider secondaryAmmo;
    [SerializeField] private float ammoDisplayDuration;

    public float CurrentShield { set { currentShield = value; } }
    public float CurrentHealth { set { currentHealth = value; } }
    public float CurrentEnergy { set { currentEnergy = value; } }

    public float CurrentPrimaryAmmo { set { currentPrimaryAmmo = value; } }
    public float CurrentSecondaryAmmo { set { currentSecondaryAmmo = value; } }

    private float currentShield;
    private float currentHealth;
    private float currentEnergy;

    private float currentPrimaryAmmo;
    private float currentSecondaryAmmo;

    private float maxShield;
    private float maxHealth;
    private float maxEnergy;

    private float maxPrimaryAmmo;
    private float maxSecondaryAmmo;

    private void Awake()
    {
        currentShield = 1;
        currentHealth = 1;
        currentEnergy = 1;
        currentPrimaryAmmo = 1;
        currentSecondaryAmmo = 1;

        maxShield = 1;
        maxHealth = 1;
        maxEnergy = 1;
        maxPrimaryAmmo = 1;
        maxSecondaryAmmo = 1;

        statusGroup.alpha = 1;
        ammoGroup.alpha = 0;
    }

    private void LateUpdate()
    {
        shieldBar.SliderValue(currentShield / maxShield);
        healthBar.SliderValue(currentHealth / maxHealth);
        energyBar.SliderValue(currentEnergy / maxEnergy);

        primaryAmmo.SliderValue(currentPrimaryAmmo / maxPrimaryAmmo, currentPrimaryAmmo);
        secondaryAmmo.SliderValue(currentSecondaryAmmo / maxSecondaryAmmo, currentSecondaryAmmo);
    }

    public void DisplayAmmoGroup(bool state)
    {
        ammoGroup.DOKill();
        float endValue = state ? 1 : 0;
        ammoGroup.DOFade(endValue, ammoDisplayDuration).SetEase(Ease.InOutSine);
    }

    public void InitializeHUDValues(SpaceshipStatsSO spaceshipStats, SpaceshipModel spaceshipModel)
    {
        maxShield = spaceshipStats.shieldMaxCapacity;
        maxHealth = spaceshipStats.hullMaxHP;
        maxEnergy = spaceshipStats.boostEnergyCapacity;

        if (spaceshipModel.UsesWeapons)
        {
            maxPrimaryAmmo = spaceshipModel.PrimaryWeapon.WeaponMaxAmmunition;
            maxSecondaryAmmo = spaceshipModel.SecondaryWeapon.WeaponMaxAmmunition;
        }
    }
}
