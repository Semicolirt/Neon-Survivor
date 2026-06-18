using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject levelUpPanel;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardParent;

    [Header("Player Stats")]
    [SerializeField] private PlayerStatsSO playerStats;

    private LevelUpManager levelUpManager;
    [SerializeField] private List<SkillDataSO> skillDataList = new List<SkillDataSO>();
    [SerializeField] private GameObject[] levelUpCards;
    [SerializeField] private Action<SkillDataSO> onSkillChosen; // Sự kiện khi người chơi chọn một kỹ năng


    public void Init(LevelUpManager levelUpManager)
    {
        this.levelUpManager = levelUpManager;
    }

    public void ShowLevelUpUI(/*List<SkillDataSO> skillOptions*/)
    {
        if (skillDataList.Count == 0)
        {
            Debug.LogWarning("Skill data list is empty! Please assign skills in the inspector.");
            return;
        }
        Time.timeScale = 0f;
        levelUpPanel.SetActive(true);
        List<SkillDataSO> selectedSkills = GetRandomUniqueSkills(3);

        ReturnCardsToPool(); // Trả các card cũ về pool trước khi tạo mới

        for (int i = 0; i < selectedSkills.Count; i++)
        {
            LevelUpCardSpawner cardSpawner = GameObject.Find("Card Spawner").GetComponent<LevelUpCardSpawner>();
            cardSpawner.SpawnLevelUpCard(cardPrefab, selectedSkills, i, cardParent);
        }
    }

    private void SelectSkill(SkillDataSO skill)
    {
        onSkillChosen?.Invoke(skill);
        levelUpPanel.SetActive(false);
    }

    private List<SkillDataSO> GetRandomUniqueSkills(int count)
    {
        List<SkillDataSO> selectedSkills = new List<SkillDataSO>();
        List<SkillDataSO> availableSkills = new List<SkillDataSO>(skillDataList);

        for (int i = 0; i < count; i++)
        {
            if (availableSkills.Count == 0) break;

            int randomIndex = UnityEngine.Random.Range(0, availableSkills.Count);
            selectedSkills.Add(availableSkills[randomIndex]);
            availableSkills.RemoveAt(randomIndex);
        }
        return selectedSkills;
    }

    public void OnSkillSelected(SkillDataSO selectedSkill)
    {
        ApplySkillToPlayerStats(selectedSkill);
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        ReturnCardsToPool(); // Trả các card về pool sau khi chọn
        if (GameManager.Instance != null && GameManager.Instance.playingState != null)
        {
            GameManager.Instance.StartChangeState(GameManager.Instance.playingState);
        }
    }

    private void ApplySkillToPlayerStats(SkillDataSO skill)
    {
        if (playerStats == null || skill == null) return;
        playerStats.damageMultiplier += skill.damageMultiplier;
        Debug.Log($"Applied skill {skill.skillName}. New damage multiplier: {playerStats.damageMultiplier}");
    }

    void ReturnCardsToPool()
    {
        LevelUpCardSpawner cardSpawner = GameObject.Find("Card Spawner").GetComponent<LevelUpCardSpawner>();
        if (cardSpawner != null)
        {
            cardSpawner.DespawnAllCards();
        }
        else
        {
            Debug.LogWarning("LevelUpCardSpawner component not found.");
        }
    }
}
