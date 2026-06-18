using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [SerializeField] private WeaponDataSO weaponData;
    [SerializeField] private InputActionAsset inputAsset;

    private InputAction fireAction;

    [Header("Fire Settings")]
    [SerializeField] private Transform firePoint;

    private float fireTimer = 0f;

    void Awake()
    {
        fireAction = inputAsset.FindActionMap("Player").FindAction("Fire");
    }

    private void OnEnable()
    {
        fireAction.Enable();
    }

    private void OnDisable()
    {
        fireAction.Disable();
    }

    void Update()
    {
        // Kiểm tra nếu nút bắn được nhấn và đã đủ thời gian giữa các lần bắn
        if (fireAction.IsPressed() && Time.time >= fireTimer)
        {
            Shoot();
            fireTimer = Time.time + weaponData.fireRate;
        }
    }

    void Shoot()
    {
        BulletSpawner bulletSpawner = FindAnyObjectByType<BulletSpawner>();
        if (bulletSpawner == null)
        {
            Debug.LogWarning("BulletSpawner component not found in the scene. Cannot shoot.");
            return;
        }
        Vector2 fireDirection = (Vector2)firePoint.right;
        bulletSpawner.SpawnBullet(fireDirection);
    }
}