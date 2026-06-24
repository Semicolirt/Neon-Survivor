using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class PlayingState : IGameState
{
    private GameManager manager;
    private LevelUpManager levelUpManager;
    private AsyncOperationHandle<SceneInstance> sceneHandle;

    public PlayingState(GameManager gameManager, LevelUpManager levelUpManager)
    {
        this.manager = gameManager;
        this.levelUpManager = levelUpManager;
    }

    private int currentWaveIndex = 0;
    private WaveManager waveManager;
    private LevelDataSO currentLevelData;
    private AsyncOperationHandle<LevelDataSO> levelDataHandle;
    public IEnumerator Enter()
    {
        if (manager.PreviousState == manager.levelUpState)
        {
            Debug.Log("Resuming from LevelUpState, skipping scene load.");
            yield break;
        }

        Debug.Log($"Addressables: Đang tải dữ liệu Level từ Key: {manager.currentLevelKey}...");
        levelDataHandle = Addressables.LoadAssetAsync<LevelDataSO>(manager.currentLevelKey);
        yield return levelDataHandle;

        if (levelDataHandle.Status == AsyncOperationStatus.Succeeded)
        {
            currentLevelData = levelDataHandle.Result;
            Debug.Log($"Addressables: Tải thành công dữ liệu Level: {currentLevelData.levelName}");
        }
        else
        {
            Debug.LogError($"Không thể tải Level Data! Vui lòng kiểm tra lại Key Addressable: '{manager.currentLevelKey}' có tồn tại không.");
            yield break;
        }

        Debug.Log("Addressables: Đang tải Scene Gameplay...");
        
        var loadOp = Addressables.LoadSceneAsync("Scene_Gameplay", LoadSceneMode.Single);
        yield return loadOp;

        if (loadOp.Status == AsyncOperationStatus.Succeeded)
        {
            sceneHandle = loadOp;
            Debug.Log("Addressables: Tải Scene Gameplay THÀNH CÔNG! Bắt đầu trận đấu.");
            
            waveManager = Object.FindAnyObjectByType<WaveManager>();
            if (waveManager != null)
            {
                waveManager.OnWaveCompleted += HandleWaveCompleted;
                currentWaveIndex = 0;
                manager.StartCoroutine(StartNextWaveWithDelay(0f));
            }
            else
            {
                Debug.LogWarning("Không tìm thấy WaveManager trong Scene Gameplay!");
            }
        }
    }

    private void HandleWaveCompleted()
    {
        currentWaveIndex++;
        if (currentLevelData != null && currentWaveIndex < currentLevelData.waves.Count)
        {
            manager.StartCoroutine(StartNextWaveWithDelay(currentLevelData.delayBetweenWaves));
        }
        else
        {
            Debug.Log("Hoàn thành tất cả các Waves! Trận đấu kết thúc.");
            if (UIVictoryManager.Instance != null)
            {
                UIVictoryManager.Instance.ShowVictoryUI();
            }
            else
            {
                Debug.LogWarning("Không tìm thấy UIVictoryManager trong Scene Gameplay!");
            }
        }
    }

    private IEnumerator StartNextWaveWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (currentLevelData != null && currentWaveIndex < currentLevelData.waves.Count)
        {
            WaveDataSO nextWave = currentLevelData.waves[currentWaveIndex];
            Debug.Log($"PlayingState: Bắt đầu Wave {currentWaveIndex + 1} - {nextWave.waveName}");
            waveManager.PlayWave(nextWave);
        }
    }

    public void Update()
    {
        // Nếu nhấn ESC, quay lại MainMenu
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     manager.StartChangeState(manager.mainMenuState);
        // }
    }

    public IEnumerator Exit()
    {
        if (manager.NextState == manager.levelUpState)
        {
            Debug.Log("Entering LevelUpState, skipping scene unload.");
            yield break;
        }

        Debug.Log("Thoát khỏi trận đấu, đang dọn dẹp Gameplay Scene...");
        
        if (waveManager != null)
        {
            waveManager.OnWaveCompleted -= HandleWaveCompleted;
        }

        if (sceneHandle.IsValid())
        {
            yield return Addressables.UnloadSceneAsync(sceneHandle);
        }

        if (levelDataHandle.IsValid())
        {
            Addressables.Release(levelDataHandle);
        }
    }
}