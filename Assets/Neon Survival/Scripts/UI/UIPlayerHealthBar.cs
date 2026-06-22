using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealthBar : MonoBehaviour, IObserver<HealthData>
{

    [SerializeField] private HealthComponent playerHealthComponent;
    [SerializeField] private Image healthBarFill;

     private void OnEnable()
    {
        if (playerHealthComponent != null)
        {
            // Đăng ký lắng nghe sự kiện khi UI được bật
            playerHealthComponent.AddObserver(this); 
        }
    }

    private void OnDisable()
    {
        if (playerHealthComponent != null)
        {
            // Bắt buộc hủy đăng ký để tránh Memory Leak
            playerHealthComponent.RemoveObserver(this);
        }
    }

    public void OnNotify(HealthData data)
    {
        if (healthBarFill != null)
        {
            // Cập nhật thanh máu dựa trên tỷ lệ
            healthBarFill.fillAmount = data.CurrentHealth / data.MaxHealth;
        }

        // Tùy chọn: Thêm hiệu ứng đổi màu hoặc nhấp nháy nếu data.CurrentHealth thấp
        if (data.IsDead)
        {
            Debug.Log("Cập nhật UI: Player đã chết!");
        }
    }
}