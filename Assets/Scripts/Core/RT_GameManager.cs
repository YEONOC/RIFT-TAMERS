using UnityEngine;
using System.IO;

/// <summary>
/// 게임 전체 상태를 관리하는 싱글톤 매니저
/// DontDestroyOnLoad 로 씬 전환 간 데이터를 유지하며 세이브/로드를 담당
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("현재 게임 상태")]
    [SerializeField] private GameState _currentState = GameState.MainMenu;

    /// <summary>현재 게임 상태</summary>
    public GameState CurrentState => _currentState;

    /// <summary>현재 세이브 데이터 (런타임 중 항상 최신 유지)</summary>
    public SaveData SaveData { get; private set; }

    private static readonly string SAVE_FILE_NAME = "save.json";
    private string SaveFilePath => Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSave();
    }

    /// <summary>게임 상태 전환</summary>
    public void SetState(GameState newState)
    {
        _currentState = newState;
    }

    /// <summary>세이브 데이터를 JSON 파일로 저장</summary>
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(SaveData, prettyPrint: true);
        File.WriteAllText(SaveFilePath, json);
        Debug.Log($"[GameManager] 게임 저장 완료: {SaveFilePath}");
    }

    /// <summary>세이브 파일 로드 (없으면 신규 SaveData 생성)</summary>
    public void LoadSave()
    {
        if (File.Exists(SaveFilePath))
        {
            string json = File.ReadAllText(SaveFilePath);
            SaveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("[GameManager] 세이브 파일 로드 완료");
        }
        else
        {
            SaveData = new SaveData();
            Debug.Log("[GameManager] 신규 게임 — 기본 세이브 데이터 생성");
        }
    }

    /// <summary>세이브 데이터 초기화 (파일 삭제 후 새 데이터 생성)</summary>
    public void DeleteSave()
    {
        if (File.Exists(SaveFilePath))
            File.Delete(SaveFilePath);

        SaveData = new SaveData();
        Debug.Log("[GameManager] 세이브 데이터 초기화 완료");
    }

    private void OnApplicationQuit()
    {
        // 런 도중 종료 시 자동 저장
        if (SaveData != null && SaveData.isRunActive)
            SaveGame();
    }
}
