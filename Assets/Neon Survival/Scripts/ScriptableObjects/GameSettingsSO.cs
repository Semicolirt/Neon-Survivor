using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings")]
public class GameSettingsSO : ScriptableObject
{
    [Header("Audio Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [Header("Graphics Settings")]
    public bool isFullScreen = true;
    public int targetFrameRate = 60;
    public bool enableVsync = true;
    
    [Header("Gameplay Settings")]
    public float baseGameSpeed = 1f;
    public bool enableScreenShake = true;
}
