using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
public class WeaponDataSO : ScriptableObject
{
    [Header("UI Info")]
    public string weaponName;
    public Sprite weaponIcon;

    [Header("Weapon Stats")]
    public float fireRate = 0.5f;


    [Header("Auto Aim Settings")]
    public bool hasAutoAim;
    public float aimRadius = 5f;
}
