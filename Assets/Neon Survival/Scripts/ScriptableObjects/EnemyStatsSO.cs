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
    public float Defense => armor;
    public float Attack => damage;
    public float MoveSpeed => moveSpeed;
}
