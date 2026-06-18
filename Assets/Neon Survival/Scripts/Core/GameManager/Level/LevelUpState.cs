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
        levelUpUIManager.ShowLevelUpUI();
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
