using UnityEngine;
using TMPro;

public class UIVictoryManager : MonoBehaviour
{
    public static UIVictoryManager Instance { get; private set; }

    [Header("UI Elements")]
    public Canvas victoryPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI goldEarnedText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Hide UI on start
        if (victoryPanel != null)
        {
            victoryPanel.gameObject.SetActive(false);
        }
    }

    public void ShowVictoryUI()
    {
        // Pause the game
        Time.timeScale = 0f;

        // Convert Score to Gold and Save
        int finalScore = 0;
        int goldEarned = 0;

        if (ScoreManager.Instance != null)
        {
            finalScore = ScoreManager.Instance.CurrentScore;
            goldEarned = ScoreManager.Instance.ConvertScoreToGold();
        }

        if (DataManager.Instance != null)
        {
            _ = DataManager.Instance.SaveDataAsync();
        }

        // Update UI Text
        if (scoreText != null)
        {
            scoreText.text = $"SCORE: {finalScore}";
        }

        if (goldEarnedText != null)
        {
            goldEarnedText.text = $"GOLD: {goldEarned}";
        }

        // Show Panel
        if (victoryPanel != null)
        {
            victoryPanel.gameObject.SetActive(true);
        }
    }

    // Assign this to the 'Main Menu' button
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Resume time before changing scene/state
        if (GameManager.Instance != null && GameManager.Instance.mainMenuState != null)
        {
            GameManager.Instance.returnToLevelSelection = true;
            GameManager.Instance.StartChangeState(GameManager.Instance.mainMenuState);
        }
    }
}
