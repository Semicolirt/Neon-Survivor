using UnityEngine;

public class HealthData
{
    public float CurrentHealth { get; }
    public float MaxHealth { get; }
    public bool IsDead { get; }

    public HealthData(float current, float max, bool isDead)
    {
        CurrentHealth = current;
        MaxHealth = max;
        IsDead = isDead;
    }
}

public class HealthComponent : Subject<HealthData>
{
    [SerializeField] private ScriptableObject statsSO;

    public float currentHealth;
    private IStats stats;

    void Awake()
    {
        stats = statsSO as IStats;
        if (stats == null)
        {
            Debug.LogError("StatsSO does not implement IStats interface.");
            return;
        }
        currentHealth = stats.MaxHealth;
    }

// Gọi khi spawn hoặc respawn để reset máu về max
    void OnEnable()
    {
        currentHealth = stats.MaxHealth;
        Notify(new HealthData(currentHealth, stats.MaxHealth, false));
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return;

        float effectiveDamage = Mathf.Max(0, damage - stats.Defense);
        currentHealth -= effectiveDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, stats.MaxHealth);

        Notify(new HealthData(currentHealth, stats.MaxHealth, currentHealth <= 0));
    }
}
