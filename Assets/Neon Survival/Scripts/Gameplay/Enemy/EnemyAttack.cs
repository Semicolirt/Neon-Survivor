using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyStatsSO enemyStatsSO;
    private HealthComponent playerHealth;
    private float damgeAmount = 10f;
    private float attackCooldown = 1f;
    private float lastAttackTime;

    void Awake()
    {
        lastAttackTime = -attackCooldown; // Cho phép tấn công ngay khi bắt đầu
        if (enemyStatsSO != null)
        {
            damgeAmount = enemyStatsSO.damage;
        }
    }
    void AttackPlayer()
    {
        // Logic tấn công người chơi
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damgeAmount);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerHealth = collision.GetComponent<HealthComponent>();
            if (playerHealth != null)
            {
                StartCoroutine(AttackCoroutine());
                collision.GetComponent<PlayerController>().OnSelfHit();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerHealth = null;
        }
    }

    IEnumerator AttackCoroutine()
    {
        while (true)
        {
            if (playerHealth != null && Time.time - lastAttackTime >= attackCooldown)
            {
                AttackPlayer();
                
                lastAttackTime = Time.time;
            }
            yield return null; // Đợi đến frame tiếp theo
        }
    }
}
