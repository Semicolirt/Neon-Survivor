using UnityEngine;

public class LoadGame : MonoBehaviour
{
    [Header("UI References")]
    public TMPro.TextMeshProUGUI goldText;
    public TMPro.TextMeshProUGUI highScoreText;

    private void Awake()   // Dùng Awake() thay vì Update()
    {
        LoadData();
    }

    private void LoadData()
    {
        if (DataManager.Instance == null)
        {
            Debug.LogError("❌ DataManager.Instance is NULL! Kiểm tra DataManager có trong scene chưa?");
            return;
        }

        DataManager.Instance.LoadData();

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (goldText != null)
            goldText.text = $"Gold: {DataManager.Instance.GameData.gold}";

        if (highScoreText != null)
            highScoreText.text = $"High Score: {DataManager.Instance.GameData.highScore}";
    }
}