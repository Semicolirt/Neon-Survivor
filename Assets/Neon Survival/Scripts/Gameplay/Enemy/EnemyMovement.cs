using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform spriteTransform;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteTransform = transform;
    }
    /// <summary>
    /// Hàm này được EnemyBrain gọi mỗi frame để tịnh tiến quái vật.
    /// </summary>
    public void Move(Vector3 direction, float speed)
    {
        // Cập nhật vị trí tịnh tiến (Ép kiểu Vector3 về Vector2 để tránh lỗi ambiguous)
        rb.MovePosition(rb.position + (Vector2)direction * speed * Time.deltaTime);

        // Lật mặt Sprite trái phải theo hướng di chuyển
        FlipSprite.Flip(spriteTransform, direction.x);
    }
}
