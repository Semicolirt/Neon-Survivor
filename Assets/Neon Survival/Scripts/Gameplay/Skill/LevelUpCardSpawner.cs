using System.Collections.Generic;
using UnityEngine;

public class LevelUpCardSpawner : MonoBehaviour
{
    [SerializeField] private Transform cardSpawnPoint;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private int initialPoolSize = 3;
    private List<LevelUpCard> activeCards = new List<LevelUpCard>();
    private LevelUpUIManager levelUpUIManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelUpUIManager = FindAnyObjectByType<LevelUpUIManager>().GetComponent<LevelUpUIManager>();
        if (ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.CreatePool(cardPrefab, initialPoolSize);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy ObjectPoolManager trong Scene!");
        }
    }

    public void SpawnLevelUpCard(GameObject cardPrefab, List<SkillDataSO> selectedSkills, int index, Transform parent)
    {
        if (cardPrefab == null) return;

        GameObject cardObj = ObjectPoolManager.Instance.Spawn(cardPrefab.gameObject, Vector3.zero, Quaternion.identity);
        if (cardObj != null && parent != null)
        {
            cardObj.transform.SetParent(parent, false);
            cardObj.transform.localScale = Vector3.one;
        }

        if (cardObj != null)
        {
            LevelUpCard card = cardObj.GetComponent<LevelUpCard>();
            if (card != null)
            {
                card.Setup(selectedSkills[index], () => levelUpUIManager.OnSkillSelected(selectedSkills[index]));
                activeCards.Add(card);
            }
        }
    }

    public void DespawnLevelUpCard(GameObject cardObj)
    {
        if (cardObj != null)
        {
            ObjectPoolManager.Instance.Despawn(cardPrefab, cardObj);
            activeCards.Remove(cardObj.GetComponent<LevelUpCard>());
        }
    }

    public void DespawnAllCards()
    {
        foreach (var card in activeCards)
        {
            if (card != null)
            {
                ObjectPoolManager.Instance.Despawn(cardPrefab, card.gameObject);
            }
        }
        activeCards.Clear();
    }
}