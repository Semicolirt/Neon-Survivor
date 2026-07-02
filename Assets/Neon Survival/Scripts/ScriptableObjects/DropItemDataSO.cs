using UnityEngine;

public enum DropItemType
{
    Health,
    Magnet,
    WeaponBoost
}

[CreateAssetMenu(fileName = "DropItemData", menuName = "ScriptableObjects/DropItemData")]
public class DropItemDataSO : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    public DropItemType itemType;
    public GameObject prefab;

    [Header("Drop Settings")]
    [Range(0f, 1f)]
    [Tooltip("Probability of dropping this item (0 to 1)")]
    public float dropChance = 0.1f;

    [Header("Item Effect Stats")]
    [Tooltip("Heal amount for Health, duration/speed value for Magnet/WeaponBoost")]
    public float value = 10f;
}
