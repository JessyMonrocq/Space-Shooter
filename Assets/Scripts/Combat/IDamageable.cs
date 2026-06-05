public interface IDamageable
{
    int CurrentShield { get; set; }
    int MaxShield { get; set; }
    int CurrentHealth { get; set; }
    int MaxHealth { get; set; }

    bool IsAlive { get; }

    void TakeDamage(int amount);

    void Heal(int amount);
}
