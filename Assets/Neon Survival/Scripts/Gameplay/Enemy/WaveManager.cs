using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Dependencies")]
    [Tooltip("Tham chiếu tới EnemySpawner để mượn logic spawn vị trí")]
    public EnemySpawner enemySpawner;

    public event Action OnWaveCompleted;

    private void Start()
    {
        if (enemySpawner == null)
        {
            enemySpawner = FindAnyObjectByType<EnemySpawner>();
        }
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

        // Danh sách để lưu các Coroutine đang chạy cho từng nhóm quái trong Wave này
        List<Coroutine> spawnCoroutines = new List<Coroutine>();

        // Chạy coroutine cho từng group quái trong đợt này
        foreach (var spawnData in waveData.enemySpawns)
        {
            spawnCoroutines.Add(StartCoroutine(SpawnEnemyGroup(spawnData)));
        }

        // Đợi cho đến khi hết thời gian của Wave
        if (waveData.waveDuration > 0)
        {
            yield return new WaitForSeconds(waveData.waveDuration);
        }
        else
        {
            // Nếu waveDuration <= 0, chờ đến khi quá trình spawn hoàn tất
            foreach (var coroutine in spawnCoroutines)
            {
                yield return coroutine;
            }
        }

        // Dọn dẹp: Hủy hoàn toàn Pool của các quái trong Wave này để giải phóng RAM
        if (ObjectPoolManager.Instance != null)
        {
            foreach (var spawnData in waveData.enemySpawns)
            {
                if (spawnData.enemyPrefab != null)
                {
                    ObjectPoolManager.Instance.DestroyPool(spawnData.enemyPrefab);
                }
            }
        }

        // Báo cho PlayingState biết Wave đã kết thúc
        OnWaveCompleted?.Invoke();
    }

    // Coroutine để spawn một nhóm quái cụ thể theo thông tin trong WaveSpawnData
    private IEnumerator SpawnEnemyGroup(WaveSpawnData spawnData)
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
    }
}
