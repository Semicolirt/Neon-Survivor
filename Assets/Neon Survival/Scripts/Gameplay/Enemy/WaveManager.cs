using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Header("Dependencies")]
    [Tooltip("Tham chiếu tới EnemySpawner để mượn logic spawn vị trí")]
    public EnemySpawner enemySpawner;

    public event Action OnWaveCompleted;

    [HideInInspector]
    public int activeEnemyCount = 0;

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
        if (enemySpawner == null)
        {
            enemySpawner = FindAnyObjectByType<EnemySpawner>();
        }
    }

    public void EnemySpawned()
    {
        activeEnemyCount++;
    }

    public void EnemyDied()
    {
        activeEnemyCount--;
    }

    public void PlayWave(WaveDataSO waveData)
    {
        StartCoroutine(WaveRoutine(waveData));
    }

    private IEnumerator WaveRoutine(WaveDataSO waveData)
    {
        // Khởi tạo Pool cho các loại quái trong Wave (nếu có ObjectPoolManager)
        if (ObjectPoolManager.Instance != null)
        {
            foreach (var spawnData in waveData.enemySpawns)
            {
                if (spawnData.enemyPrefab != null)
                {
                    ObjectPoolManager.Instance.CreatePool(spawnData.enemyPrefab, spawnData.count);
                }
            }
        }

        // Theo dõi số lượng nhóm quái đã spawn xong
        int completedSpawns = 0;
        int totalSpawns = waveData.enemySpawns.Count;

        List<Coroutine> spawnCoroutines = new List<Coroutine>();

        // Chạy coroutine cho từng group quái trong đợt này
        foreach (var spawnData in waveData.enemySpawns)
        {
            spawnCoroutines.Add(StartCoroutine(SpawnEnemyGroup(spawnData, () => completedSpawns++)));
        }

        float timer = waveData.waveDuration;

        if (waveData.isBossWave)
        {
            // Boss Wave: Chờ spawn xong toàn bộ và bắt buộc tiêu diệt hết quái
            yield return new WaitUntil(() => completedSpawns >= totalSpawns);
            yield return new WaitUntil(() => activeEnemyCount <= 0);
        }
        else
        {
            // Normal Wave:
            if (timer > 0)
            {
                while (timer > 0)
                {
                    timer -= Time.deltaTime;
                    // Nếu đã spawn xong và không còn quái nào sống -> kết thúc sớm
                    if (completedSpawns >= totalSpawns && activeEnemyCount <= 0)
                    {
                        break;
                    }
                    yield return null;
                }
            }
            else
            {
                // Nếu không có thời gian giới hạn, chờ spawn xong và tiêu diệt hết quái
                yield return new WaitUntil(() => completedSpawns >= totalSpawns);
                yield return new WaitUntil(() => activeEnemyCount <= 0);
            }
        }

        // Dọn dẹp: Dừng các coroutine spawn chưa chạy xong của wave hiện tại
        foreach (var coroutine in spawnCoroutines)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
        spawnCoroutines.Clear();

        // Dọn dẹp: Force-despawn tất cả quái còn sống mà KHÔNG tính vào activeEnemyCount
        EnemyController[] aliveEnemies = UnityEngine.Object.FindObjectsByType<EnemyController>(
            UnityEngine.FindObjectsSortMode.None);
        foreach (var enemy in aliveEnemies)
        {
            if (enemy.gameObject.activeSelf)
            {
                enemy.ForceDeactivate();
            }
        }

        // Reset về 0 để đảm bảo Wave tiếp theo bắt đầu với count sạch
        activeEnemyCount = 0;

        // Đợi 1 frame để mọi OnDisable xử lý xong trước khi chuyển Wave
        yield return null;

        // Báo cho PlayingState biết Wave đã kết thúc
        OnWaveCompleted?.Invoke();
    }

    // Coroutine để spawn một nhóm quái cụ thể theo thông tin trong WaveSpawnData
    private IEnumerator SpawnEnemyGroup(WaveSpawnData spawnData, Action onComplete)
    {
        for (int i = 0; i < spawnData.count; i++)
        {
            if (enemySpawner != null)
            {
                if (spawnData.enemyPrefab != null)
                {
                    enemySpawner.SpawnSpecificEnemy(spawnData.enemyPrefab);
                }else
                {
                    Debug.LogWarning("Enemy prefab is null in WaveSpawnData. Skipping spawn.");
                }
            }

            if (spawnData.spawnRate > 0)
            {
                yield return new WaitForSeconds(spawnData.spawnRate);
            }
        }

        onComplete?.Invoke();
    }
}
