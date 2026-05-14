using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    [Header("Health Component Settings")]
    private int shieldMaxCapacity;
    private float shieldCooldown;
    private float shieldRechargeRate;
    private int hullMaxHP;

    public int MaxShield { get; set; }
    public int CurrentShield { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public bool IsAlive => CurrentHealth > 0;

    protected virtual void Start()
    {
        CurrentShield = shieldMaxCapacity;
        CurrentHealth = hullMaxHP;
    }

    public void TakeDamage(int ammount)
    {
        if (!IsAlive)
        {
            return;
        }

        if (CurrentShield > 0)
        {
            CurrentShield -= ammount;
            CurrentShield = Mathf.Max(CurrentShield, 0);
        } else
        {
            CurrentHealth -= ammount;
            CurrentHealth = Mathf.Max(CurrentHealth, 0);
        }

        if (CurrentHealth <= 0)
        {
            OnDeath();
        }
    }

    public void ChargeShield(int ammount)
    {
        if (!IsAlive)
        {
            return;
        }

        CurrentShield += ammount;
        CurrentShield = Mathf.Min(CurrentShield, shieldMaxCapacity);
    }

    public void Heal(int ammount)
    {
        if (!IsAlive)
        {
            return;
        }

        CurrentHealth += ammount;
        CurrentHealth = Mathf.Min(CurrentHealth, hullMaxHP);
    }

    protected virtual void OnDeath()
    {

    }
}
