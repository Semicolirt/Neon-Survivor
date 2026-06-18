using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Cần thêm thư viện này để dùng Button
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class MainMenuState : IGameState
{
    private GameManager manager;
    private AsyncOperationHandle<SceneInstance> sceneHandle;
    private Button playButton; // Biến lưu trữ Button UI

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

            // ---- ĐOẠN ĐĂNG KÝ SỰ KIỆN BUTTON UI ----
            // Tìm Button trong Scene vừa load lên thông qua Tên GameObject
            GameObject btnObject = GameObject.Find("PlayButton"); 
            
            if (btnObject != null)
            {
                playButton = btnObject.GetComponent<Button>();
                if (playButton != null)
                {
                    // Lắng nghe sự kiện Click, khi Click thì gọi hàm OnPlayButtonClicked
                    playButton.onClick.AddListener(OnPlayButtonClicked);
                }
            }
            else
            {
                Debug.LogError("Không tìm thấy GameObject nào tên là 'PlayButton' trong Scene MainMenu!");
            }
        }
    }

    public void Update()
    {
        // Hàm Update bây giờ có thể để trống, không cần check Input.GetKeyDown nữa!
    }

    // Hàm xử lý logic khi người chơi click chuột vào Button UI
    private void OnPlayButtonClicked()
    {
        Debug.Log("Button Play đã được Click! Đang chuyển sang PlayingState...");
        manager.StartChangeState(manager.playingState);
    }

    public IEnumerator Exit()
    {
        Debug.Log("Rời khỏi MainMenu, đang giải phóng Scene...");
        
        // Hủy lắng nghe sự kiện trước khi xóa Scene để tránh lỗi bộ nhớ
        if (playButton != null)
        {
            playButton.onClick.RemoveListener(OnPlayButtonClicked);
        }

        if (sceneHandle.IsValid())
        {
            yield return Addressables.UnloadSceneAsync(sceneHandle);
        }
    }
}