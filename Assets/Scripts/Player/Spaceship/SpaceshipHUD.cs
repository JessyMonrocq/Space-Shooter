using UnityEngine;
using UnityEngine.UI;

public class SpaceshipHUD : MonoBehaviour
{
    [Header("HUD Settings")]
    [SerializeField] private Slider shieldBar;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider energyBar;

    public float CurrentShield { set { currentShield = value; } }
    public float CurrentHealth { set { currentHealth = value; } }
    public float CurrentEnergy { set { currentEnergy = value; } }
    public float MaxShield { set { maxShield = value; } }
    public float MaxHealth { set { maxHealth = value; } }
    public float MaxEnergy { set { maxEnergy = value; } }

    private float currentShield;
    private float currentHealth;
    private float currentEnergy;

    private float maxShield;
    private float maxHealth;
    private float maxEnergy;

    private void Start()
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
        shieldBar.value = currentShield / maxShield;
        healthBar.value = currentHealth / maxHealth;
        energyBar.value = currentEnergy / maxEnergy;
    }
}
