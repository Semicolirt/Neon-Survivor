using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Danh sách các prefab quái có thể sinh ra")]
    public GameObject[] enemyPrefabs;
    
    [Tooltip("Mục tiêu để spawn quái xung quanh (thường là Player)")]
    public Transform playerTransform;

    [Tooltip("Thời gian giữa 2 lần sinh quái (giây)")]
    public float spawnInterval = 1f;

    [Tooltip("Khoảng cách sinh quái so với vị trí Player")]
    public float spawnRadius = 10f;

    [Tooltip("Số lượng quái khởi tạo sẵn trong Pool để chống lag")]
    public int initialPoolSize = 30;

    [Tooltip("Bật/tắt tự động sinh quái (Update)")]
    public bool autoSpawn = false; // Tắt mặc định để WaveManager quản lý

    private float spawnTimer;

    private void Start()
    {
        // Khởi tạo Pool cho tất cả các loại quái
        if (ObjectPoolManager.Instance != null)
        {
            foreach (GameObject enemyPrefab in enemyPrefabs)
            {
                ObjectPoolManager.Instance.CreatePool(enemyPrefab, initialPoolSize);
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy ObjectPoolManager trong Scene!");
        }

        // Tự động tìm Player nếu chưa gán
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
    }

    private void Update()
    {
        //Chạy khi Auto Spawn được bật, dùng cho hàm SpawnEnemy() test trực tiếp trong PlayingState
        if (!autoSpawn || playerTransform == null || enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        spawnTimer += Time.deltaTime;
        
        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    //Dùng trực tiếp trong PlayingState (Test)
    private void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedEnemy = enemyPrefabs[randomIndex];

        Vector3 spawnPosition = GetRandomSpawnPosition();

        if (ObjectPoolManager.Instance != null)
        {
            GameObject spawnedEnemy = ObjectPoolManager.Instance.Spawn(selectedEnemy, spawnPosition, Quaternion.identity);
            EnemyStats enemyStats = spawnedEnemy.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.OnSpawn(selectedEnemy);
            }
            else
            {
                Debug.LogWarning($"Prefab {selectedEnemy.name} không có component EnemyStats. Vui lòng thêm để hỗ trợ Object Pooling.");
            }
        }
        else
        {
            Instantiate(selectedEnemy, spawnPosition, Quaternion.identity);
        }
    }


    // Hàm này sẽ được gọi từ WaveManager để spawn quái cụ thể theo từng nhóm trong đợt (Chính thức)
    public void SpawnSpecificEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab == null) return;
        
        Vector3 spawnPosition = GetRandomSpawnPosition();

        if (ObjectPoolManager.Instance != null)
        {
            GameObject spawnedEnemy = ObjectPoolManager.Instance.Spawn(enemyPrefab, spawnPosition, Quaternion.identity);
            EnemyStats enemyStats = spawnedEnemy.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.OnSpawn(enemyPrefab);
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy ObjectPoolManager trong Scene! Đang sử dụng Instantiate thay thế, có thể gây lag nếu spawn nhiều quái.");
        }
    }

    //Lấy vị trí ngẫu nhiên spawn quái xung quanh Player trong bán kính spawnRadiusd
    private Vector3 GetRandomSpawnPosition()
    {
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        
        float x = Mathf.Cos(randomAngle);
        float y = Mathf.Sin(randomAngle);

        Vector3 direction = new Vector3(x, y, 0).normalized; 

        return playerTransform.position + direction * spawnRadius;
    }

    private void OnDrawGizmosSelected()
    {
        if (playerTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerTransform.position, spawnRadius);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }
}
