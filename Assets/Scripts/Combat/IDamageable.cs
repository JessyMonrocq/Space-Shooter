public interface IDamageable
{
    int CurrentShield { get; set; }
    int MaxShield { get; }
    int CurrentHealth { get; set; }
    int MaxHealth { get; }

    bool IsAlive { get; }

    void TakeDamage(int amount);

    void Heal(int amount);
}
