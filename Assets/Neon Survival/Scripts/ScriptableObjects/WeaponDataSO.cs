using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
public class WeaponDataSO : ScriptableObject
{
    [Header("UI Info")]
    public string weaponName;
    public Sprite weaponIcon;

    [Header("Weapon Stats")]
    public float fireRate = 0.5f;


    [Header("Audio")]
    [Tooltip("Âm thanh khi bắn")]
    public AudioClip shootSound;

    [Range(0f, 1f)]
    [Tooltip("Âm lượng tiếng bắn")]
    public float shootVolume = 0.5f;

    [Tooltip("Pitch ngẫu nhiên min-max để tạo biến thể âm thanh")]
    public Vector2 shootPitchRange = new Vector2(0.9f, 1.1f);

    [Header("Auto Aim Settings")]
    public bool hasAutoAim;
    public float aimRadius = 5f;
}
