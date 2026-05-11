using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    [Header("Health Component Settings")]
    [SerializeField] private int shieldMaxCapacity = 50;
    [SerializeField] private float shieldCooldown = 1f;
    [SerializeField] private float shieldRechargeRate = 1f;
    [SerializeField] private int hullMaxHP = 100;

    public int MaxShield => shieldMaxCapacity;
    public int CurrentShield { get; set; }
    public int MaxHealth => hullMaxHP;
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
