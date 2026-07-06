using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class Shooting : MonoBehaviour
{
    [SerializeField] private WeaponDataSO weaponData;
    [SerializeField] private InputActionAsset inputAsset;

    private InputAction fireAction;

    [Header("Fire Settings")]
    [SerializeField] private Transform firePoint;

    private float fireTimer = 0f;
    private float fireRateMultiplier = 1f;

    private AudioSource audioSource;

    void Awake()
    {
        fireAction = inputAsset.FindActionMap("Player").FindAction("Fire");

        // Khởi tạo AudioSource cho tiếng bắn
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        fireAction.Enable();
    }

    private void OnDisable()
    {
        fireAction.Disable();
    }

    public void SetFireRateMultiplier(float multiplier)
    {
        fireRateMultiplier = multiplier;
    }

    void Update()
    {
        // Kiểm tra nếu nút bắn được nhấn và đã đủ thời gian giữa các lần bắn
        if (fireAction.IsPressed() && Time.time >= fireTimer)
        {
            Shoot();
            fireTimer = Time.time + (weaponData.fireRate * fireRateMultiplier);
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

        // Phát âm thanh bắn
        PlayShootSound();
    }

    private void PlayShootSound()
    {
        if (weaponData.shootSound == null || audioSource == null) return;

        // Random pitch để tạo biến thể âm thanh, tránh nghe lặp lại đơn điệu
        audioSource.pitch = Random.Range(weaponData.shootPitchRange.x, weaponData.shootPitchRange.y);
        audioSource.PlayOneShot(weaponData.shootSound, weaponData.shootVolume);
    }
}