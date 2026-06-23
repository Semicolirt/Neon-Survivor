using System;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager Instance { get; private set; }

    [SerializeField] private PlayerStatsSO playerStats;
    [SerializeField] private LevelUpUIManager levelUpUIManager;

    [Header("Run-time Level Data")]
    [SerializeField] private float currentExp;
    [SerializeField] private float expToNextLevel;
    [SerializeField] private int currentLevel;

    private void Initialize()
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Initialize();
        if (levelUpUIManager != null)
        {
            levelUpUIManager.Init(this);
        }
    }

    void Start()
    {
        currentLevel = playerStats.level;
        currentExp = playerStats.currentExperience;
        expToNextLevel = playerStats.experienceToNextLevel;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddExperience(float amount)
    {
        currentExp += amount;
        playerStats.currentExperience = (int)currentExp;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            currentLevel++;
            playerStats.level = currentLevel;
            // Tăng yêu cầu EXP cho level tiếp theo (ví dụ: tăng 20% mỗi level)
            expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.2f);
            playerStats.experienceToNextLevel = (int)expToNextLevel;
            Debug.Log($"Level Up! New Level: {currentLevel}, EXP to Next Level: {expToNextLevel}");
            if (GameManager.Instance != null && GameManager.Instance.levelUpState != null)
            {
                GameManager.Instance.StartChangeState(GameManager.Instance.levelUpState);
            }
            else
            {
                levelUpUIManager.ShowLevelUpUI();
            }
        }
    }
}
