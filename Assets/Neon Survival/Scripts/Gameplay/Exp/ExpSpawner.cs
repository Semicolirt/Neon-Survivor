using UnityEngine;

public class ExpSpawner : MonoBehaviour
{

    [Header("Exp Settings")]
    [Tooltip("Danh sách các prefab Exp có thể sinh ra")]
    public GameObject expPrefab;
    // [Tooltip("Mục tiêu enemy để spawn Exp xung quanh")]
    // public GameObject targetEnemy;
    [Tooltip("Thời gian giữa 2 lần sinh Exp (giây)")]
    public float spawnInterval = 5f;
    [Tooltip("Bán kính sinh Exp so với vị trí của Enemy bị tiêu diệt")]
    public float spawnRadius = 2f;

    private int initialPoolSize = 20; // Số lượng Exp khởi tạo sẵn trong Pool để chống lag

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Khoi tạo Pool cho tất cả các loại Exp
        if (ObjectPoolManager.Instance != null)
        {

            ObjectPoolManager.Instance.CreatePool(expPrefab, initialPoolSize);

        }
        else
        {
            Debug.LogWarning("Không tìm thấy ObjectPoolManager trong Scene!");
        }
    }

    public void DropExp(GameObject targetEnemy)
    {
        if (expPrefab == null || targetEnemy == null) return;

        // Tính toán vị trí spawn ngẫu nhiên xung quanh Enemy đã bị tiêu diệt
        Vector2 spawnPosition = (Vector2)targetEnemy.transform.position;

        // Spawn Exp từ Object Pool
        GameObject exp = ObjectPoolManager.Instance.Spawn(expPrefab, spawnPosition, Quaternion.identity);
        
        if (exp != null)
        {
            //gọi hàm khởi tạo ExpOrb
            ExpOrb expOrb = exp.GetComponent<ExpOrb>();
            if (expOrb != null)            {
                expOrb.OnSpawn(spawnPosition); // Kích hoạt hiệu ứng nảy khi spawn
            }

            float expValue = targetEnemy.GetComponent<EnemyController>().experience; // Lấy giá trị EXP từ Enemy
            expOrb.SetExperience(expValue); // Truyền giá trị EXP từ Enemy sang ExpOrb
        }
    }

    void OnDrawGizmosSelected()
    {
        // Vẽ bán kính spawn Exp trong Scene view để dễ dàng điều chỉnh
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
