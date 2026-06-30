using UnityEngine;

/// <summary>
/// Thanh máu của Player.
/// Tự động tìm HealthComponent trên cùng GameObject (hoặc assign thủ công qua Inspector).
/// Xử lý thêm logic hiển thị khi player chết.
/// </summary>
public class UIPlayerHealthBar : UIHealthBarBase
{
    [Header("Player Health Bar - Settings")]
    [Tooltip("Để trống để tự tìm HealthComponent trên Player bằng tag")]
    [SerializeField] private HealthComponent playerHealthComponent;

    // =========================================================
    //  Override - Cung cấp HealthComponent
    // =========================================================

    protected override HealthComponent GetHealthComponent()
    {
        if (playerHealthComponent != null)
            return playerHealthComponent;

        // Tự tìm Player theo tag nếu không được assign sẵn
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerHealthComponent = player.GetComponent<HealthComponent>();
            if (playerHealthComponent == null)
                Debug.LogWarning("[UIPlayerHealthBar] Không tìm thấy HealthComponent trên Player.");
        }
        else
        {
            Debug.LogWarning("[UIPlayerHealthBar] Không tìm thấy GameObject có tag 'Player'.");
        }

        return playerHealthComponent;
    }

    // =========================================================
    //  Override - Xử lý thêm khi máu thay đổi
    // =========================================================

    protected override void OnHealthChanged(HealthData data)
    {
        if (data.IsDead)
        {
            Debug.Log("[UIPlayerHealthBar] Player đã chết — có thể ẩn thanh máu hoặc kích hoạt hiệu ứng.");
        }
    }
}