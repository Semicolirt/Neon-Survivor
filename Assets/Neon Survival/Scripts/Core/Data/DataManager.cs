using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public GameData GameData { get; private set; }
    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Path.Combine(Application.persistentDataPath, "GameData.json");
            LoadData(); // Load synchronously on start to ensure data is ready
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                GameData = JsonUtility.FromJson<GameData>(json);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load GameData: " + e.Message);
                GameData = new GameData();
            }
        }
        else
        {
            GameData = new GameData();
        }
    }

    public async Task SaveDataAsync()
    {
        try
        {
            string json = JsonUtility.ToJson(GameData, true); // true for pretty print
            
            // Write async to avoid blocking main thread
            using (StreamWriter writer = new StreamWriter(saveFilePath, false))
            {
                await writer.WriteAsync(json);
            }
            Debug.Log("GameData saved successfully at " + saveFilePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save GameData: " + e.Message);
        }
    }
}
