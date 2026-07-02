using UnityEngine;
using TMPro;

/// <summary>
/// Thanh máu của Boss.
/// Hỗ trợ cả World-Space (gắn trực tiếp vào prefab Boss)
/// và Screen-Space (gắn trên HUD và tự bind với Boss hiện tại).
/// Hiển thị thêm: tên Boss, số máu dạng text.
/// </summary>
public class UIBossHealthBar : UIHealthBarBase
{
    [Header("Boss Health Bar - Settings")]
    [Tooltip("Assign trực tiếp nếu thanh máu nằm trên cùng prefab Boss.\n" +
             "Để trống nếu thanh máu là HUD Screen-Space và sẽ bind sau bằng BindToBoss().")]
    [SerializeField] private HealthComponent bossHealthComponent;

    [Header("Boss UI Elements")]
    [SerializeField] private TMP_Text bossNameText;
    [SerializeField] private TMP_Text healthValueText; // Hiển thị "1500 / 3000"
    [SerializeField] private GameObject bossHUDRoot;   // Panel cha để Show/Hide

    [SerializeField] private Vector3 originalScale = new Vector3(1,1,1);

    void Start()
    {
        // Nếu bossHealthComponent đã được assign sẵn trên Inspector (World-Space)
        // thì bind luôn; nếu không thì sẽ được bind động qua BindToBoss()
        if (bossHealthComponent != null)
            bossHealthComponent.AddObserver(this);

        ShowHUD(bossHealthComponent != null);
    }

    void LateUpdate()
    {
        transform.localScale = originalScale; // Giữ nguyên scale để tránh bị ảnh hưởng bởi Canvas Scale
    }

    public void BindToBoss(HealthComponent bossHealth, string bossName = "BOSS")
    {
        // Hủy đăng ký observer cũ nếu có
        if (bossHealthComponent != null)
            bossHealthComponent.RemoveObserver(this);

        bossHealthComponent = bossHealth;

        if (bossHealthComponent != null)
            bossHealthComponent.AddObserver(this);

        if (bossNameText != null)
            bossNameText.text = bossName;

        ShowHUD(true);
    }

    public void UnbindBoss()
    {
        if (bossHealthComponent != null)
        {
            bossHealthComponent.RemoveObserver(this);
            bossHealthComponent = null;
        }

        ShowHUD(false);
    }

    protected override HealthComponent GetHealthComponent()
    {
        return bossHealthComponent;
    }

    protected override void OnHealthChanged(HealthData data)
    {
        // Hiển thị "CurrentHealth / MaxHealth"
        if (healthValueText != null)
            healthValueText.text = $"{Mathf.CeilToInt(data.CurrentHealth)} / {Mathf.CeilToInt(data.MaxHealth)}";

        if (data.IsDead)
        {
            Debug.Log("[UIBossHealthBar] Boss đã chết — ẩn thanh máu.");
            UnbindBoss();
        }
    }

    private void ShowHUD(bool show)
    {
        if (bossHUDRoot != null)
            bossHUDRoot.SetActive(show);
    }
}
