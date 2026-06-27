using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private IGameState currentState;
    [SerializeField] private LevelUpUIManager levelUpUIManager;
    [SerializeField] private LevelUpManager levelUpManager;
    private bool isTransitioning = false; // Chống bug người chơi spam nút chuyển state liên tục

    // Các State
    public MainMenuState mainMenuState { get; private set; }
    public PlayingState playingState { get; private set; }
    public LevelUpState levelUpState { get; private set; }

    [Header("Game Data")]
    [Tooltip("Key Addressable của Level hiện tại (VD: Level_1_Data, Level_2_Data)")]
    public string currentLevelKey = "Level_1_Data";
    
    [HideInInspector]
    public bool returnToLevelSelection = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Khởi tạo các State
        mainMenuState = new MainMenuState(this);
        playingState = new PlayingState(this, levelUpManager);
        levelUpState = new LevelUpState(this, levelUpUIManager);  
    }

    private void Start()
    {
        // Bắt đầu game với Main Menu
        StartChangeState(mainMenuState);
    }

    private void Update()
    {
        // Chỉ chạy Update của State khi KHÔNG trong quá trình load chuyển màn
        if (currentState != null && !isTransitioning)
        {
            currentState.Update();
        }
    }

    public IGameState NextState { get; private set; }
    public IGameState PreviousState { get; private set; }

    // Hàm trung gian để các State khác gọi khi muốn đổi trạng thái
    public void StartChangeState(IGameState newState)
    {
        if (currentState == newState || isTransitioning) return;
        
        NextState = newState;

        // Chạy Coroutine chuyển đổi
        StartCoroutine(ChangeStateRoutine(newState));
    }

    private IEnumerator ChangeStateRoutine(IGameState newState)
    {
        isTransitioning = true;

        // 1. Thoát và dọn dẹp Scene cũ
        if (currentState != null)
        {
            yield return currentState.Exit();
        }

        PreviousState = currentState;

        // 2. Thay đổi trạng thái
        currentState = newState;

        // 3. Bước vào trạng thái mới và đợi load Scene mới qua Addressables
        if (currentState != null)
        {
            yield return currentState.Enter();
        }

        isTransitioning = false;
    }
}