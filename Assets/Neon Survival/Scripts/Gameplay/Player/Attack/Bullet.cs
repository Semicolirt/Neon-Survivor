using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D[] hitResults = new Collider2D[20]; // Buffer để lưu kết quả va chạm
    [SerializeField] private LayerMask enemyLayer;
    [Header("Bullet Data")]
    public BulletDataSO bulletData;
    private float timerBullet;

    // ← BIẾN QUAN TRỌNG: Lưu Prefab gốc khi spawn
    [SerializeField] private GameObject originalPrefab;
    [SerializeField] private PlayerStatsSO playerStats; // Tham chiếu đến PlayerStatsSO để lấy thông tin về damage

    /// <summary>
    /// Gọi khi ObjectPool spawn viên đạn
    /// </summary>
    public void OnSpawn(GameObject prefab, Vector2 fireDirection)
    {
        originalPrefab = prefab;                    // Lưu prefab gốc
        timerBullet = bulletData != null ? bulletData.lifetime : 5f;

        rb = GetComponent<Rigidbody2D>();
        if (rb != null && bulletData != null)
        {
            rb.linearVelocity = fireDirection * bulletData.speed;
        }
    }

    void Update()
    {
        timerBullet -= Time.deltaTime;
        if (timerBullet <= 0f)
        {
            DeSpawnBullet();
        }
    }

    [System.Obsolete]
    void FixedUpdate()
    {
        OnHitEnemy(); // Kiểm tra va chạm với enemy mỗi frame (có thể tối ưu sau này)
    }

    [System.Obsolete]
    void OnHitEnemy()
    {
        // Logic khi bắn trúng enemy, có thể gọi TakeDamage() hoặc các hiệu ứng khác
        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, bulletData.hitRadius, hitResults, enemyLayer);
        for (int i = 0; i < hitCount; i++)
        {
            EnemyController enemyController = hitResults[i].GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.OnSelfHit(); // Gọi hiệu ứng hit trên EnemyController (nếu có)
            }
            HealthComponent enemyHealth = hitResults[i].GetComponent<HealthComponent>();
            if (enemyHealth != null)
            {
                float finalDamage = bulletData.damage * (1f + (playerStats != null ? playerStats.damageMultiplier : 0f)); // Có thể thêm logic tính toán damage dựa trên các yếu tố khác
                enemyHealth.TakeDamage(finalDamage);
            }
            DeSpawnBullet(); // Despawn viên đạn sau khi va chạm
        }
    }

    void DeSpawnBullet()
    {
        if (ObjectPoolManager.Instance != null)
        {
            // Ưu tiên dùng originalPrefab, fallback về bulletPrefab nếu cần
            GameObject prefabToDespawn = originalPrefab;

            if (prefabToDespawn != null)
            {
                ObjectPoolManager.Instance.Despawn(prefabToDespawn, gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Reset khi despawn (tốt cho lần spawn sau)
    public void OnDespawn()
    {
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }
}