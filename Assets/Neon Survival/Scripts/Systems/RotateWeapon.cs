using UnityEngine;
using UnityEngine.InputSystem;

public class RotateWeapon : MonoBehaviour
{
    private PlayerController playerController;

    void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateWeaponTowardsMouse();
    }

    private void RotateWeaponTowardsMouse()
    {
        // Logic lấy vị trí chuột thế giới và xoay WeaponPivot theo hướng chuột
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 lookDir = (mousePos - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        if (playerController != null) // Nếu player đang lật ngược, điều chỉnh góc
        {
            if (playerController.transform.localScale.x < 0)
            {
                angle += 180f;
            }
        }
        else
        {
            Debug.LogWarning("PlayerController component not found on the GameObject. Weapon rotation may not work correctly.");
        }
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

}
