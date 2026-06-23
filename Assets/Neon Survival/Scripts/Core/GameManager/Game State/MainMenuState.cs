using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class MainMenuState : IGameState
{
    private GameManager manager;
    private AsyncOperationHandle<SceneInstance> sceneHandle;

    public MainMenuState(GameManager gameManager)
    {
        this.manager = gameManager;
    }

    public IEnumerator Enter()
    {
        Debug.Log("Addressables: Đang tải Scene MainMenu...");
        
        var loadOp = Addressables.LoadSceneAsync("Scene_MainMenu", LoadSceneMode.Single);
        yield return loadOp;

        if (loadOp.Status == AsyncOperationStatus.Succeeded)
        {
            sceneHandle = loadOp;
            Debug.Log("Addressables: Tải Scene MainMenu THÀNH CÔNG!");
        }
    }

    public void Update()
    {
        
    }

    public IEnumerator Exit()
    {
        Debug.Log("Rời khỏi MainMenu, đang giải phóng Scene...");

        if (sceneHandle.IsValid())
        {
            yield return Addressables.UnloadSceneAsync(sceneHandle);
        }
    }
}