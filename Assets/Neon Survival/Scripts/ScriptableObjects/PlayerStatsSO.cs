using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStatsSO : ScriptableObject, IStats
{
    [Header("Basic Stats")]
    public float maxHealth = 100f;
    public float moveSpeed = 5f;
    public float damageMultiplier = 1f;
    public float armor = 0f;
    
    [Header("Progression")]
    public int level = 1;
    public int currentExperience = 0;
    public int experienceToNextLevel = 100;

    public float MaxHealth => maxHealth;
    public float Defense => armor;
    public float Attack => damageMultiplier;
    public float MoveSpeed => moveSpeed;
}
