using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaveSpawnData
{
    [Tooltip("The enemy prefab to spawn")]
    public GameObject enemyPrefab;
    
    [Tooltip("How many enemies to spawn in this group")]
    public int count;
    
    [Tooltip("Time between each spawn in this group")]
    public float spawnRate;
}

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/WaveData")]
public class WaveDataSO : ScriptableObject
{
    [Header("Wave Info")]
    public string waveName;
    public float waveDuration; // optional, could be 0 if wave ends when all enemies are dead
    public bool isBossWave; // Nếu true, hoàn thành wave này sẽ kết thúc màn chơi

    [Header("Spawns")]
    [Tooltip("List of enemy groups to spawn during this wave")]
    public List<WaveSpawnData> enemySpawns;
}
