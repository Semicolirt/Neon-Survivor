using System.Collections;
using UnityEngine;

public class LevelUpState : IGameState
{
    private GameManager manager;
    private LevelUpUIManager levelUpUIManager;
    
    public LevelUpState(GameManager gameManager, LevelUpUIManager levelUpUIManager)
    {
        this.manager = gameManager;
        this.levelUpUIManager = levelUpUIManager;
    }
    public IEnumerator Enter()
    {
        Debug.Log("Entering LevelUpState...");
        
        if (levelUpUIManager == null)
        {
            levelUpUIManager = Object.FindObjectOfType<LevelUpUIManager>();
        }

        if (levelUpUIManager != null)
        {
            levelUpUIManager.ShowLevelUpUI();
        }
        else
        {
            Debug.LogError("LevelUpUIManager không tìm thấy trong Scene!");
        }

        yield break;
    }

    public IEnumerator Exit()
    {
        Debug.Log("Exiting LevelUpState...");
        yield break;
    }

    public void Update()
    {
        Debug.Log("Updating LevelUpState...");
        
    }
}
