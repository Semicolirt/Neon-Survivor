using System.Collections.Generic;
using UnityEngine;

public class DropItemSpawner : MonoBehaviour
{
    public static DropItemSpawner Instance { get; private set; }

    [Header("Drop Items Configuration")]
    [SerializeField] private List<DropItemDataSO> dropItemList = new List<DropItemDataSO>();
    [SerializeField] private int initialPoolSize = 5;

    [Header("Global Drop Modifier")]
    [Range(0f, 1f)]
    [Tooltip("Overall chance that any item will drop when an enemy dies")]
    [SerializeField] private float globalDropRate = 0.3f;

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

    private void Start()
    {
        // Initialize pool for each unique drop item prefab
        if (ObjectPoolManager.Instance != null)
        {
            HashSet<GameObject> uniquePrefabs = new HashSet<GameObject>();
            foreach (var item in dropItemList)
            {
                if (item != null && item.prefab != null && !uniquePrefabs.Contains(item.prefab))
                {
                    uniquePrefabs.Add(item.prefab);
                    ObjectPoolManager.Instance.CreatePool(item.prefab, initialPoolSize);
                }
            }
        }
        else
        {
            Debug.LogWarning("ObjectPoolManager not found in the scene! DropItemSpawner cannot initialize pools.");
        }
    }

    /// <summary>
    /// Attempts to drop a random item based on their configured rates.
    /// </summary>
    public void TryDropItem(Vector3 spawnPosition)
    {
        if (dropItemList == null || dropItemList.Count == 0) return;

        // Roll global check first to see if anything drops at all
        if (Random.value > globalDropRate) return;

        // Calculate total weight of all active drops in the list
        float totalWeight = 0f;
        List<DropItemDataSO> validDrops = new List<DropItemDataSO>();

        foreach (var item in dropItemList)
        {
            if (item != null && item.prefab != null)
            {
                totalWeight += item.dropChance;
                validDrops.Add(item);
            }
        }

        if (validDrops.Count == 0 || totalWeight <= 0f) return;

        // Roll a value between 0 and totalWeight
        float roll = Random.Range(0f, totalWeight);
        float currentSum = 0f;

        foreach (var item in validDrops)
        {
            currentSum += item.dropChance;
            if (roll <= currentSum)
            {
                SpawnItem(item, spawnPosition);
                break;
            }
        }
    }

    private void SpawnItem(DropItemDataSO item, Vector3 position)
    {
        if (ObjectPoolManager.Instance == null) return;

        GameObject spawnedObj = ObjectPoolManager.Instance.Spawn(item.prefab, position, Quaternion.identity);
        if (spawnedObj != null)
        {
            DropItem dropItem = spawnedObj.GetComponent<DropItem>();
            if (dropItem != null)
            {
                dropItem.OnSpawn(item, item.prefab, position);
            }
        }
    }
}
