using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyStats")]
public class EnemyStatsSO : ScriptableObject, IStats
{
    [Header("Basic Stats")]
    public string enemyName;
    public float maxHealth;
    public float moveSpeed;
    public float damage;
    public float armor;
    
    [Header("Reward")]
    public float experience;

    public float MaxHealth => maxHealth;
    public float Armor => armor;
    public float Damage => damage;
    public float MoveSpeed => moveSpeed;
}
