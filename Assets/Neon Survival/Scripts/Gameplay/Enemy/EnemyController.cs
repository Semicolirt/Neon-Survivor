using System.Collections;
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
    private Collider2D enemyCollider;
    private bool isSpawned = false;
    // Dùng Awake để nạp dữ liệu từ ScriptableObject
    void Awake()
    {
        if (experienceData != null)
        {
            experience = experienceData.expValue;
        }
        healthComponent = GetComponent<HealthComponent>();
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
    }

    // Gọi khi EnemySpawner spawn enemy này, set lai máu đầy và lưu prefab gốc để sau này Despawn đúng pool 
    public void OnSpawn(GameObject prefab)
    {
        isSpawned = true;
        animator.SetBool("Dead", false); // Reset trạng thái chết khi spawn lại
        enemyCollider.enabled = true; // Kích hoạt collider khi spawn lại
        if (healthComponent != null)
        {
            healthComponent.AddObserver(this);
        }
        originalPrefab = prefab; // Lưu lại prefab gốc khi spawn

        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.EnemySpawned();
        }
    }

    void OnDisable()
    {
        if (healthComponent != null)
        {
            healthComponent.RemoveObserver(this); // Hủy đăng ký observer khi enemy bị vô hiệu hóa
        }

        if (isSpawned)
        {
            isSpawned = false;
            if (WaveManager.Instance != null)
            {
                WaveManager.Instance.EnemyDied();
            }
        }
    }

    // Gọi bởi WaveManager khi kết thúc Wave để force-despawn quái mà không tính vào activeEnemyCount
    public void ForceDeactivate()
    {
        isSpawned = false; // Tắt flag trước để OnDisable không gọi EnemyDied()
        if (originalPrefab != null && ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.Despawn(originalPrefab, gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Hàm xử lý cái chết (Chết giả)
    private void Die()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(10);
        }

        // TODO: Xử lý rớt kinh nghiệm (EXP), rớt đồ, sinh ra Fx máu... tại đây
        ExpSpawner expSpawner = FindAnyObjectByType<ExpSpawner>();
        if (expSpawner != null)
        {
            expSpawner.DropExp(gameObject); //Truyền gameObject này để spawn EXP xung quanh vị trí của nó
        }

        StartCoroutine(DespawnAfterDeathAnimation());
    }

    IEnumerator DespawnAfterDeathAnimation()
    {
        // Chờ cho đến khi animation "Dead" kết thúc
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
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
            enemyCollider.enabled = false; // Vô hiệu hóa collider để tránh va chạm sau khi chết
            animator.SetBool("Dead", true);
            Die();
        }
    }
}
