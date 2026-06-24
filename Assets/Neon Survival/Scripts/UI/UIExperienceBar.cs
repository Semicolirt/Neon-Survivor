using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIExperienceBar : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image expBarFill;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Start()
    {
        if (LevelUpManager.Instance != null)
        {
            // Subscribe to the experience changed event
            LevelUpManager.Instance.OnExperienceChanged += UpdateExpBar;
            
            // Initialize the UI with current values on start
            UpdateExpBar(LevelUpManager.Instance.CurrentExp, LevelUpManager.Instance.ExpToNextLevel);
        }
        else
        {
            Debug.LogWarning("UIExperienceBar: LevelUpManager Instance is null. Make sure it is loaded.");
        }
    }

    private void OnDestroy()
    {
        if (LevelUpManager.Instance != null)
        {
            // Unsubscribe to prevent memory leaks when UI is destroyed
            LevelUpManager.Instance.OnExperienceChanged -= UpdateExpBar;
        }
    }

    private void UpdateExpBar(float currentExp, float expToNextLevel)
    {
        // Update the fill amount of the experience bar
        if (expBarFill != null && expToNextLevel > 0)
        {
            expBarFill.fillAmount = currentExp / expToNextLevel;
        }
        
        // Update the level text if available
        if (LevelUpManager.Instance != null)
        {
             UpdateLevelText(LevelUpManager.Instance.CurrentLevel);
        }
    }

    private void UpdateLevelText(int currentLevel)
    {
        if (levelText != null)
        {
            levelText.text = $"LEVEL {currentLevel}";
        }
    }
}
