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
    public float Armor => armor;
    public float Damage => damageMultiplier;
    public float MoveSpeed => moveSpeed;

    //Reset stats to default values when the ScriptableObject is enabled
    void OnEnable()
    {
        maxHealth = 100f;
        moveSpeed = 5f;
        damageMultiplier = 1f;
        armor = 0f;
    }
}
