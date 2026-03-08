using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 메인 메뉴 씬 UI 컨트롤러.
/// Canvas 에 부착하며, 자식 버튼에 onClick 이벤트를 바인딩한다.
/// </summary>
public class RT_MainMenuUI : MonoBehaviour
{
    private void Start()
    {
        BindButton("ButtonPanel/Btn_NewGame",  OnNewGameClicked);
        BindButton("ButtonPanel/Btn_Continue", OnContinueClicked);
        BindButton("ButtonPanel/Btn_Settings", OnSettingsClicked);
        BindButton("ButtonPanel/Btn_Quit",     OnQuitClicked);

        // 진행 중인 런이 없으면 계속하기 버튼 비활성화
        var continueBtn = transform.Find("ButtonPanel/Btn_Continue")?.GetComponent<Button>();
        if (continueBtn != null && GameManager.Instance != null)
            continueBtn.interactable = GameManager.Instance.SaveData.isRunActive;
    }

    // ─────────────────────────────────────────
    // 버튼 콜백
    // ─────────────────────────────────────────

    private void OnNewGameClicked()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.DeleteSave();   // 기존 런 초기화 후 새 게임 시작
        GameManager.Instance.StartNewRun();
    }

    private void OnContinueClicked()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.SaveData.isRunActive)
            GameManager.Instance.ContinueRun();
        else
            Debug.Log("[MainMenuUI] 진행 중인 런이 없습니다.");
    }

    private void OnSettingsClicked()
    {
        // TODO: 설정 패널 열기 (Milestone 3 예정)
        Debug.Log("[MainMenuUI] 설정 화면은 아직 구현되지 않았습니다.");
    }

    private void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ─────────────────────────────────────────
    // 헬퍼
    // ─────────────────────────────────────────

    private void BindButton(string path, UnityEngine.Events.UnityAction callback)
    {
        var btn = transform.Find(path)?.GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(callback);
        else
            Debug.LogWarning($"[MainMenuUI] 버튼을 찾을 수 없음: {path}");
    }
}
