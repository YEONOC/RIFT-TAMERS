using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 전체 상태를 관리하는 싱글톤 매니저.
/// DontDestroyOnLoad 로 씬 전환 간 데이터를 유지하며 세이브/로드와 씬 전환을 담당.
/// </summary>
public class RT_GameManager : MonoBehaviour
{
    // ─────────────────────────────────────────
    // 싱글톤
    // ─────────────────────────────────────────
    public static RT_GameManager Instance { get; private set; }

    [Header("현재 게임 상태")]
    [SerializeField] private GameState _currentState = GameState.MainMenu;

    /// <summary>현재 게임 상태</summary>
    public GameState CurrentState => _currentState;

    /// <summary>현재 세이브 데이터 (런타임 중 항상 최신 유지)</summary>
    public SaveData SaveData { get; private set; }

    private static readonly string SAVE_FILE_NAME = "save.json";
    private string SaveFilePath => Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

    private bool _isTransitioning;

    // ─────────────────────────────────────────
    // 초기화
    // ─────────────────────────────────────────

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

    // ─────────────────────────────────────────
    // 게임 상태 제어
    // ─────────────────────────────────────────

    /// <summary>게임 상태 전환</summary>
    public void SetState(GameState newState)
    {
        _currentState = newState;
    }

    // ─────────────────────────────────────────
    // 씬 전환 퍼블릭 API
    // ─────────────────────────────────────────

    /// <summary>메인 메뉴로 이동</summary>
    public void GoToMainMenu()
    {
        SetState(GameState.MainMenu);
        LoadScene(GameConstants.SCENE_MAIN_MENU);
    }

    /// <summary>새 런을 시작하고 던전으로 이동</summary>
    public void StartNewRun()
    {
        SaveData.isRunActive  = true;
        SaveData.currentZone  = 1;
        SaveData.currentFloor = 1;
        SaveData.runSeed      = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        SetState(GameState.InRun);
        GameEvents.RaiseRunStarted();
        LoadScene(GameConstants.SCENE_DUNGEON);
    }

    /// <summary>런을 종료하고 메타 허브로 이동</summary>
    public void EndRun(bool isCleared = false)
    {
        SaveData.isRunActive = false;
        SaveGame();
        GameEvents.RaiseRunEnded(isCleared);
        SetState(GameState.MetaHub);
        LoadScene(GameConstants.SCENE_META_HUB);
    }

    /// <summary>메타 허브에서 기존 런을 이어서 던전으로 이동</summary>
    public void ContinueRun()
    {
        SetState(GameState.InRun);
        LoadScene(GameConstants.SCENE_DUNGEON);
    }

    // ─────────────────────────────────────────
    // 내부 씬 로드 처리
    // ─────────────────────────────────────────

    private void LoadScene(int sceneIndex)
    {
        if (_isTransitioning) return;
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        _isTransitioning = true;

        // 씬 전환 전 이벤트 리스너 정리 (메모리 누수 방지)
        GameEvents.ClearAllListeners();

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
        op.allowSceneActivation = false;

        // 로딩 90% 도달 대기
        while (op.progress < 0.9f)
            yield return null;

        // TODO: 페이드 아웃 연출 추가 예정
        op.allowSceneActivation = true;

        yield return new WaitUntil(() => op.isDone);

        _isTransitioning = false;
    }

    // ─────────────────────────────────────────
    // 세이브 / 로드
    // ─────────────────────────────────────────

    /// <summary>세이브 데이터를 JSON 파일로 저장</summary>
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(SaveData, prettyPrint: true);
        File.WriteAllText(SaveFilePath, json);
        Debug.Log($"[RT_GameManager] 게임 저장 완료: {SaveFilePath}");
    }

    /// <summary>세이브 파일 로드 (없으면 신규 SaveData 생성)</summary>
    public void LoadSave()
    {
        if (File.Exists(SaveFilePath))
        {
            string json = File.ReadAllText(SaveFilePath);
            SaveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("[RT_GameManager] 세이브 파일 로드 완료");
        }
        else
        {
            SaveData = new SaveData();
            Debug.Log("[RT_GameManager] 신규 게임 — 기본 세이브 데이터 생성");
        }
    }

    /// <summary>세이브 데이터 초기화 (파일 삭제 후 새 데이터 생성)</summary>
    public void DeleteSave()
    {
        if (File.Exists(SaveFilePath))
            File.Delete(SaveFilePath);

        SaveData = new SaveData();
        Debug.Log("[RT_GameManager] 세이브 데이터 초기화 완료");
    }

    private void OnApplicationQuit()
    {
        // 런 도중 종료 시 자동 저장
        if (SaveData != null && SaveData.isRunActive)
            SaveGame();
    }
}
