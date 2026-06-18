using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelDataSO : ScriptableObject
{
    [Header("Level Information")]
    public string levelName;
    
    [Header("Wave Settings")]
    [Tooltip("Danh sách các Wave trong Level này")]
    public List<WaveDataSO> waves;
    
    [Tooltip("Thời gian nghỉ giữa các Wave (giây)")]
    public float delayBetweenWaves = 5f;
}
