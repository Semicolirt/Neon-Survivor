using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelSelectionPanel;
    
    [Header("Main Menu Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    [Header("Level Selection Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button[] levelButtons; // Thay thế bằng một mảng các button

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        
        // Trạng thái hiển thị ban đầu
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (levelSelectionPanel != null) levelSelectionPanel.SetActive(false);

        // Đăng ký sự kiện
        if (playButton != null) playButton.onClick.AddListener(ShowLevelSelection);
        if (backButton != null) backButton.onClick.AddListener(ShowMainMenu);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
        
        // Duyệt qua tất cả các button trong mảng levelButtons
        if (levelButtons != null)
        {
            foreach (var btn in levelButtons)
            {
                if (btn != null)
                {
                    btn.onClick.AddListener(() => OnLevelButtonClicked(btn));
                }
            }
        }
    }

    private void OnLevelButtonClicked(Button clickedButton)
    {
        string levelKey = "";

        // Tìm component TextMeshProUGUI bên trong Button
        var tmpText = clickedButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (tmpText != null)
        {
            // Lấy TÊN CỦA GAMEOBJECT chứa component Text(TMP) thay vì nội dung chữ
            levelKey = tmpText.gameObject.name;
        }
        else
        {
            // Fallback: Tìm UI Text cơ bản của Unity
            var stdText = clickedButton.GetComponentInChildren<Text>();
            if (stdText != null)
            {
                levelKey = stdText.gameObject.name;
            }
        }

        levelKey = levelKey.Trim(); // Loại bỏ khoảng trắng thừa
        
        if (!string.IsNullOrEmpty(levelKey))
        {
            StartLevel(levelKey);
        }
        else
        {
            Debug.LogError("MainMenuUIManager: Không tìm thấy GameObject Text nào bên trong Button!");
        }
    }

    private void ShowLevelSelection()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (levelSelectionPanel != null) levelSelectionPanel.SetActive(true);
    }

    private void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (levelSelectionPanel != null) levelSelectionPanel.SetActive(false);
    }

    private void StartLevel(string levelKey)
    {
        if (gameManager != null)
        {
            gameManager.currentLevelKey = levelKey;
            gameManager.StartChangeState(gameManager.playingState);
        }
        else
        {
            Debug.LogError("MainMenuUIManager: GameManager instance is null!");
        }
    }

    private void QuitGame()
    {
        Debug.Log("Quit Game Requested");
        Application.Quit();
    }
}
