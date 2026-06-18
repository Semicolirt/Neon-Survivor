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

    public static void HandlePlayerDeath(Animator animator, Rigidbody2D rb)
    {
        //animator.SetTrigger("Die");
        rb.linearVelocity = Vector2.zero; // Dừng chuyển động khi chết
        Debug.Log("Player has died. Implement death handling logic here.");
        
    }
}
