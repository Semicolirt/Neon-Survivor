using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelUpCard : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image backgroundImage;     // Frame hoặc nền card

    private SkillDataSO currentSkill;
    private System.Action onClickAction;   // Callback khi click

    // Gọi từ LevelUpManager để setup dữ liệu
    public void Setup(SkillDataSO skill, System.Action onSelected)
    {
        currentSkill = skill;
        onClickAction = onSelected;

        if (skill == null) return;

        if (iconImage != null) iconImage.sprite = skill.icon;
        if (titleText != null) titleText.text = skill.skillName;
        if (descriptionText != null) descriptionText.text = skill.description;

        // Tự động gán sự kiện click cho Button (tránh việc quên kéo thả trong Inspector)
        Button btn = GetComponent<Button>();
        if (btn == null)
        {
            btn = gameObject.AddComponent<Button>();
        }
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnCardClicked);
    }

    // Gọi từ Button OnClick
    public void OnCardClicked()
    {
        onClickAction?.Invoke();
    }
}