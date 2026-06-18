using UnityEngine;

public static class FlipSprite
{
    public static void Flip(Transform transform, float directionX)
    {
        // Nếu đi sang phải
        if (directionX > 0.01f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        // Nếu đi sang trái
        else if (directionX < -0.01f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
