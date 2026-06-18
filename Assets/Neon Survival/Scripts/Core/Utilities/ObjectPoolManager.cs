using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    // Lưu trữ các pool. Key là InstanceID của Prefab để phân biệt.
    private Dictionary<EntityId, Queue<GameObject>> poolDictionary = new Dictionary<EntityId, Queue<GameObject>>();

    // Lưu trữ cha của các object trong hierarchy cho gọn

    private Dictionary<EntityId, Transform> poolParents = new Dictionary<EntityId, Transform>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Khởi tạo Pool cho một Prefab cụ thể
    /// </summary>
    public void CreatePool(GameObject prefab, int initialSize)
    {
        EntityId poolKey = prefab.GetEntityId();

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<GameObject>());

            // Tạo một GameObject rỗng làm cha để Hierarchy gọn gàng hơn
            GameObject parentObj = new GameObject(prefab.name + "_Pool");
            parentObj.transform.SetParent(transform);
            poolParents.Add(poolKey, parentObj.transform);

            for (int i = 0; i < initialSize; i++)
            {
                CreateNewObject(prefab, poolKey);
            }
        }
    }

    /// <summary>
    /// Sinh ra (Lấy) một object từ Pool
    /// </summary>
    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        EntityId poolKey = prefab.GetEntityId();

        // Nếu pool chưa tồn tại, tự động tạo mới với size = 1
        if (!poolDictionary.ContainsKey(poolKey))
        {
            Debug.Log($"Pool cho {prefab.name} chưa tồn tại. Tự động khởi tạo.");
            CreatePool(prefab, 1);
        }

        Queue<GameObject> pool = poolDictionary[poolKey];

        // Nếu trong pool hết hàng, tạo thêm
        if (pool.Count == 0)
        {
            CreateNewObject(prefab, poolKey);
        }

        // Lấy object ra khỏi queue
        GameObject objectToSpawn = pool.Dequeue();


        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    /// <summary>
    /// Trả object về lại Pool (Thay vì Destroy)
    /// </summary>
    public void Despawn(GameObject prefab, GameObject obj)
    {
        EntityId poolKey = prefab.GetEntityId();
        if (poolDictionary.ContainsKey(poolKey))
        {
            obj.SetActive(false);
            poolDictionary[poolKey].Enqueue(obj);
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy pool cho {obj.name}. Tiến hành Destroy bình thường.");
        }
    }

    /// <summary>
    /// Thu hồi toàn bộ object đang active của một Prefab về lại Pool
    /// </summary>
    public void DespawnAllActiveObjects(GameObject prefab)
    {
        if (prefab == null) return;


        EntityId poolKey = prefab.GetEntityId();

        if (poolDictionary.ContainsKey(poolKey) && poolParents.ContainsKey(poolKey))
        {
            Transform parentTransform = poolParents[poolKey];
            Queue<GameObject> pool = poolDictionary[poolKey];

            // Duyệt qua tất cả các con của object cha (chứa các clone)
            foreach (Transform child in parentTransform)
            {
                if (child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(false);
                    pool.Enqueue(child.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Xóa hoàn toàn một Pool khỏi bộ nhớ (Hủy object cha và toàn bộ clone, dọn dẹp Dictionary)
    /// </summary>
    public void DestroyPool(GameObject prefab)
    {
        if (prefab == null) return;


        EntityId poolKey = prefab.GetEntityId();

        if (poolParents.ContainsKey(poolKey))
        {
            // Tiêu diệt object cha sẽ kéo theo toàn bộ các object con bị tiêu diệt
            Destroy(poolParents[poolKey].gameObject);
            poolParents.Remove(poolKey);
        }

        if (poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary[poolKey].Clear();
            poolDictionary.Remove(poolKey);
        }
    }

    // Hàm phụ trợ tạo object mới
    private void CreateNewObject(GameObject prefab, EntityId poolKey)
    {
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(false);
        newObj.transform.SetParent(poolParents[poolKey]);
        poolDictionary[poolKey].Enqueue(newObj);
    }
}
