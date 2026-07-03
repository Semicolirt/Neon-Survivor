using UnityEngine;

[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(SeparateSystem))]
public class EnemyBrain : MonoBehaviour
{
    private Transform playerTransform;
    private EnemyController enemyController;
    private EnemyMovement enemyMovement;

    private float moveSpeed;
    private bool canMove = true; // Ví dụ: dùng để làm choáng quái

    [Header("Optimization")]
    private float updatePathInterval = 0.5f; // Tối ưu: Cập nhật hướng đi mỗi 0.2s
    private float pathTimer;
    private Vector2 currentDirection;
    private HealthComponent healthComponent;
    private SeparateSystem separateSystem;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        enemyMovement = GetComponent<EnemyMovement>();
        healthComponent = GetComponent<HealthComponent>();
        separateSystem = GetComponent<SeparateSystem>();
    }

    private void OnEnable()
    {
        if (enemyController != null && enemyController.enemyStats != null)
        {
            moveSpeed = enemyController.enemyStats.moveSpeed;
        }

        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
        
        canMove = true; // Khôi phục trạng thái di chuyển khi hồi sinh
        pathTimer = updatePathInterval; // Gán bằng interval để nó chạy tìm hướng ngay lập tức khi vừa spawn
    }

    private void Update()
    {
        // Các điều kiện dừng di chuyển (chết, bị choáng...)
        if (!canMove || healthComponent.currentHealth <= 0) 
            return;

        if (playerTransform != null)
        {
            // Tối ưu: Chỉ tính toán hướng đi (phép toán Vector tốn kém) mỗi khoảng thời gian nhất định
            pathTimer += Time.deltaTime;
            if (pathTimer >= updatePathInterval)
            {
                currentDirection = (playerTransform.position - transform.position).normalized;
                pathTimer = 0f;
            }

            Vector2 separationForce = separateSystem.GetCurrentSeparationForce();

            // Tính toán vận tốc cuối cùng (đã bao gồm tốc độ di chuyển)
            Vector2 finalVelocity = currentDirection * moveSpeed + separationForce;

            // Truyền finalVelocity và hướng nhìn (currentDirection.x) để tránh giật lật sprite
            enemyMovement.Move(finalVelocity, currentDirection.x);
        }
    }

    // Gọi hàm này từ các script khác (ví dụ script kỹ năng của người chơi) để làm choáng quái
    public void SetCanMove(bool state)
    {
        canMove = state;
    }
}
