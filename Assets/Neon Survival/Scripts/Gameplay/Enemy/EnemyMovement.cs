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
    public void Move(Vector2 velocity, float facingDirectionX)
    {
        // Cập nhật vị trí tịnh tiến với vận tốc tổng hợp
        rb.MovePosition(rb.position + velocity * Time.deltaTime);

        // Lật mặt Sprite trái phải theo hướng mục tiêu chính (bỏ qua lực đẩy để tránh giật sprite)
        FlipSprite.Flip(spriteTransform, facingDirectionX);
    }
}
