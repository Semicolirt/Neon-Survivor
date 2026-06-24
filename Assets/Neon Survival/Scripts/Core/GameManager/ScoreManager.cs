using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText; // Reference to the UI Text component for displaying the score

    public int CurrentScore { get; private set; }

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
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
        SetScoreText();
    }

    public int ConvertScoreToGold()
    {
        if (DataManager.Instance == null) return 0;

        // 10 score = 1 gold
        int goldEarned = CurrentScore / 10;
        
        // Cập nhật vàng
        DataManager.Instance.GameData.gold += goldEarned;

        // Cập nhật High Score nếu lớn hơn
        if (CurrentScore > DataManager.Instance.GameData.highScore)
        {
            DataManager.Instance.GameData.highScore = CurrentScore;
        }

        Debug.Log($"Game Over! Score: {CurrentScore}, Gold Earned: {goldEarned}, Total Gold: {DataManager.Instance.GameData.gold}");
        return goldEarned;
    }

    private void SetScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"SCORE: {CurrentScore}";
        }
    }
}
