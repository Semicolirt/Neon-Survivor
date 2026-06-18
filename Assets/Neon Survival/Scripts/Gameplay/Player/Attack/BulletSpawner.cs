using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("Bullet Settings")]
    [Tooltip("Prefab viên đạn để spawn")]
    public GameObject bulletPrefab;

    [Tooltip("Điểm xuất hiện của viên đạn (thường là vị trí súng)")]
    public Transform firePoint;

    [Tooltip("Số lượng viên đạn khởi tạo sẵn trong Pool để chống lag")]
    public int initialPoolSize = 50;

    private Transform playerTransform;

    void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
    }

    private void Start()
    {
        // Khởi tạo Pool cho viên đạn
        if (ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.CreatePool(bulletPrefab, initialPoolSize);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy ObjectPoolManager trong Scene!");
        }
        // Tự động tìm firePoint nếu chưa gán
        if (firePoint == null)
        {
            Transform potentialFirePoint = transform.Find("FirePoint");
            if (potentialFirePoint != null)
            {
                firePoint = potentialFirePoint;
            }
        }
    }

    public void SpawnBullet(Vector2 fireDirection)
    {
        if (bulletPrefab == null || firePoint == null) return;
        Quaternion fireRotation = Quaternion.LookRotation(Vector3.forward, fireDirection);

        GameObject bullet = ObjectPoolManager.Instance.Spawn(bulletPrefab, firePoint.position, fireRotation);

        if (bullet != null)
        {
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                if(playerTransform.localScale.x < 0) // Nếu player đang quay sang trái, đảo ngược hướng bắn
                {
                    fireDirection = -fireDirection;
                }
                bulletScript.OnSpawn(bulletPrefab, fireDirection);
            }
        }
    }
}