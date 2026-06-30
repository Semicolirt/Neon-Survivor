using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Abstract base class cho mọi loại thanh máu trong game.
/// Sử dụng Observer Pattern để nhận cập nhật từ HealthComponent.
/// Kế thừa class này để tạo thanh máu cho Player, Boss, v.v.
/// </summary>
public abstract class UIHealthBarBase : MonoBehaviour, IObserver<HealthData>
{
    [Header("Health Bar UI")]
    [SerializeField] protected Image fillImage;
    [SerializeField] protected Image delayedFillImage; // "Ghost" fill để tạo hiệu ứng giảm chậm

    [Header("Smooth Animation")]
    [SerializeField] protected bool useSmoothFill = true;
    [SerializeField] protected float fillSmoothSpeed = 5f;

    [Header("Color Thresholds")]
    [SerializeField] protected bool useColorGradient = true;
    [SerializeField] protected Color colorFull   = new Color(0.2f, 0.9f, 0.3f); // Xanh lá
    [SerializeField] protected Color colorMid    = new Color(1f,   0.8f, 0f);   // Vàng
    [SerializeField] protected Color colorLow    = new Color(1f,   0.2f, 0.1f); // Đỏ

    [Header("Thresholds (0-1)")]
    [SerializeField] [Range(0f, 1f)] protected float midThreshold = 0.5f;
    [SerializeField] [Range(0f, 1f)] protected float lowThreshold = 0.25f;

    // Target fill để smooth lerp tới
    private float targetFillAmount  = 1f;
    private float delayedFillAmount = 1f;
    private const float DelayedShrinkSpeed = 1.5f; // Tốc độ thu ghost bar

    protected HealthComponent healthComponent;

    // =========================================================
    //  Vòng đời Unity
    // =========================================================

    protected virtual void Awake()
    {
        healthComponent = GetHealthComponent();
    }

    protected virtual void OnEnable()
    {
        if (healthComponent != null)
            healthComponent.AddObserver(this);
    }

    protected virtual void OnDisable()
    {
        if (healthComponent != null)
            healthComponent.RemoveObserver(this);
    }

    protected virtual void Update()
    {
        if (!useSmoothFill) return;

        // Smooth fill chính
        if (fillImage != null)
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFillAmount, Time.deltaTime * fillSmoothSpeed);

        // Ghost fill thu chậm hơn fill chính
        if (delayedFillImage != null)
        {
            if (delayedFillAmount > targetFillAmount)
                delayedFillAmount = Mathf.Lerp(delayedFillAmount, targetFillAmount, Time.deltaTime * DelayedShrinkSpeed);
            delayedFillImage.fillAmount = delayedFillAmount;
        }
    }

    // =========================================================
    //  Observer callback
    // =========================================================

    public void OnNotify(HealthData data)
    {
        float ratio = data.MaxHealth > 0f ? data.CurrentHealth / data.MaxHealth : 0f;
        SetFillTarget(ratio);
        OnHealthChanged(data);
    }

    // =========================================================
    //  Internal helpers
    // =========================================================

    private void SetFillTarget(float ratio)
    {
        targetFillAmount = Mathf.Clamp01(ratio);

        if (!useSmoothFill && fillImage != null)
            fillImage.fillAmount = targetFillAmount;

        // Ghost bar chỉ dịch chuyển khi fill giảm
        if (delayedFillImage != null && targetFillAmount > delayedFillAmount)
            delayedFillAmount = targetFillAmount;

        if (useColorGradient && fillImage != null)
            fillImage.color = EvaluateHealthColor(targetFillAmount);
    }

    private Color EvaluateHealthColor(float ratio)
    {
        if (ratio > midThreshold)
            return Color.Lerp(colorMid, colorFull, (ratio - midThreshold) / (1f - midThreshold));
        else if (ratio > lowThreshold)
            return Color.Lerp(colorLow, colorMid, (ratio - lowThreshold) / (midThreshold - lowThreshold));
        else
            return colorLow;
    }

    // =========================================================
    //  Abstract / Virtual - Subclass override
    // =========================================================

    /// <summary>
    /// Subclass phải cung cấp HealthComponent cần lắng nghe.
    /// Được gọi một lần trong Awake.
    /// </summary>
    protected abstract HealthComponent GetHealthComponent();

    /// <summary>
    /// Hook để subclass xử lý thêm logic khi nhận HealthData
    /// (ví dụ: hiển thị số máu, hiệu ứng nhấp nháy, ẩn/hiện panel chết, v.v.)
    /// </summary>
    protected virtual void OnHealthChanged(HealthData data) { }
}
