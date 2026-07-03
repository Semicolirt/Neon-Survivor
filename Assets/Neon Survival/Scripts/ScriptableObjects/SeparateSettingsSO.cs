using UnityEngine;

[CreateAssetMenu(menuName = "Neon Survivor/Separation Settings", fileName = "New Separation Settings")]
public class SeparateSettingsSO : ScriptableObject
{
    [Header("Separation Settings")]
    [Tooltip("Bán kính phát hiện unit khác")]
    public float separationRadius = 1.8f;

    [Tooltip("Độ mạnh lực đẩy")]
    public float separationWeight = 2.0f;

    [Tooltip("Tốc độ update (giây). 0 = mỗi FixedUpdate")]
    public float updateInterval = 0.05f;   // Tùy chọn tối ưu
}