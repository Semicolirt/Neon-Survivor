using UnityEngine;

public class EnemyController : MonoBehaviour, IObserver<HealthData>
{
    [Header("Enemy Data")]
    public EnemyStatsSO enemyStats;
    public ExperienceSO experienceData;
    public float experience;

    private GameObject originalPrefab;
    private HealthComponent healthComponent;
    private Animator animator;
    // Dùng Awake để nạp dữ liệu từ ScriptableObject
    void Awake()
    {
        if (experienceData != null)
        {
            experience = experienceData.expValue;
        }
        healthComponent = GetComponent<HealthComponent>();
        animator = GetComponent<Animator>();
    }

    // Gọi khi EnemySpawner spawn enemy này, set lai máu đầy và lưu prefab gốc để sau này Despawn đúng pool 
    public void OnSpawn(GameObject prefab)
    {
        if (healthComponent != null)
        {
            healthComponent.AddObserver(this);
        }
        originalPrefab = prefab; // Lưu lại prefab gốc khi spawn
    }

    void OnDisable()
    {
        if (healthComponent != null)
        {
            healthComponent.RemoveObserver(this); // Hủy đăng ký observer khi enemy bị vô hiệu hóa
        }
    }

    // Hàm xử lý cái chết (Chết giả)
    private void Die()
    {
        // TODO: Xử lý rớt kinh nghiệm (EXP), rớt đồ, sinh ra Fx máu... tại đây
        ExpSpawner expSpawner = FindAnyObjectByType<ExpSpawner>();
        if (expSpawner != null)
        {
            expSpawner.DropExp(gameObject); //Truyền gameObject này để spawn EXP xung quanh vị trí của nó
        }

        GameObject prefabToDespawn = originalPrefab;
        if (originalPrefab != null)
        {
            ObjectPoolManager.Instance.Despawn(prefabToDespawn, gameObject);
        }
    }

    public void OnSelfHit()
    {
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
    }

    public void OnNotify(HealthData data)
    {
        if (data.IsDead)
        {
            Die();
        }
    }
}
