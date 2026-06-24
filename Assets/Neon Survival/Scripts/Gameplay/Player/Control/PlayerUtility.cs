using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUtility
{
    public static void FlipPlayerTowardMouse(Transform spriteTransform)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (mousePosition.x < spriteTransform.position.x)
        {
            FlipSprite.Flip(spriteTransform, -1);
        }
        else
        {
            FlipSprite.Flip(spriteTransform, 1);
        }
    }

    public static void HandlePlayerDeath(Animator animator, Rigidbody2D rb, GameObject weapon, Canvas deadCanvas, Canvas playerCanvas)
    {
        rb.linearVelocity = Vector2.zero; // Dừng chuyển động khi chết
        animator.SetTrigger("Dead");
        if (weapon != null)
        {
            weapon.SetActive(false); // Vô hiệu hóa vũ khí khi chết
        }
        if (deadCanvas != null)
        {
            deadCanvas.gameObject.SetActive(true); // Hiển thị Canvas khi player chết
        }
        if (playerCanvas != null)
        {
            playerCanvas.gameObject.SetActive(false); // Ẩn Canvas của player khi chết
        }
        Time.timeScale = 0f; // Tạm dừng game khi player chết
    }
}
