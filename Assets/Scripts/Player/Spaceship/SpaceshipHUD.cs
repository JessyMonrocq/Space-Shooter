using UnityEngine;
using UnityEngine.UI;

public class SpaceshipHUD : MonoBehaviour
{
    [Header("HUD Settings")]
    [SerializeField] private CustomSlider shieldBar;
    [SerializeField] private CustomSlider healthBar;
    [SerializeField] private CustomSlider energyBar;

    public float CurrentShield { set { currentShield = value; } }
    public float CurrentHealth { set { currentHealth = value; } }
    public float CurrentEnergy { set { currentEnergy = value; } }

    private float currentShield;
    private float currentHealth;
    private float currentEnergy;

    private float maxShield;
    private float maxHealth;
    private float maxEnergy;

    private void Awake()
    {
        currentShield = 1;
        currentHealth = 1;
        currentEnergy = 1;

        maxShield = 1;
        maxHealth = 1;
        maxEnergy = 1;
    }

    private void LateUpdate()
    {
        shieldBar.SliderValue(currentShield / maxShield);
        healthBar.SliderValue(currentHealth / maxHealth);
        energyBar.SliderValue(currentEnergy / maxEnergy);
    }

    public void InitializeHUDValues(SpaceshipStatsSO spaceshipStats)
    {
        maxShield = spaceshipStats.shieldMaxCapacity;
        maxHealth = spaceshipStats.hullMaxHP;
        maxEnergy = spaceshipStats.boostEnergyCapacity;
    }
}
