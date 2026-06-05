using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    public event Action<int, int, int> OnShieldDamageTaken;
    public event Action<int, int, int> OnHealthDamageTaken;

    [Header("Health Component Settings")]
    [SerializeField] private int maxShield;
    [SerializeField] private float shieldCooldown;
    [SerializeField] private float shieldRechargeRate;
    [SerializeField] private int maxHealth;

    public int MaxShield { get; set; }
    public int CurrentShield { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public bool IsAlive => CurrentHealth > 0;

    protected virtual void Start()
    {
        CurrentShield = maxShield;
        CurrentHealth = maxHealth;
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

            OnShieldDamageTaken.Invoke(ammount, CurrentShield, maxShield);
        } else
        {
            CurrentHealth -= ammount;
            CurrentHealth = Mathf.Max(CurrentHealth, 0);

            OnHealthDamageTaken.Invoke(ammount, CurrentHealth, maxHealth);
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
        CurrentShield = Mathf.Min(CurrentShield, maxShield);
    }

    public void Heal(int ammount)
    {
        if (!IsAlive)
        {
            return;
        }

        CurrentHealth += ammount;
        CurrentHealth = Mathf.Min(CurrentHealth, maxHealth);
    }

    protected virtual void OnDeath()
    {

    }
}
