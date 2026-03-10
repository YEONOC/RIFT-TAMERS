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
        BindButton("ButtonPanel/Btn_Settings", OnSettingsClicked);
        BindButton("ButtonPanel/Btn_Quit",     OnQuitClicked);

        // 계속하기 버튼은 interactable 설정이 필요해 직접 참조 보관
        var continueBtn = BindButton("ButtonPanel/Btn_Continue", OnContinueClicked);
        if (continueBtn != null && RT_GameManager.Instance != null)
            continueBtn.interactable = RT_GameManager.Instance.SaveData.isRunActive;
    }

    // ─────────────────────────────────────────
    // 버튼 콜백
    // ─────────────────────────────────────────

    private void OnNewGameClicked()
    {
        if (RT_GameManager.Instance == null) return;
        RT_GameManager.Instance.DeleteSave();   // 기존 런 초기화 후 새 게임 시작
        RT_GameManager.Instance.StartNewRun();
    }

    private void OnContinueClicked()
    {
        if (RT_GameManager.Instance == null) return;

        if (RT_GameManager.Instance.SaveData.isRunActive)
            RT_GameManager.Instance.ContinueRun();
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

    /// <summary>버튼을 찾아 콜백을 등록하고 Button 컴포넌트를 반환 (추가 설정에 사용)</summary>
    private Button BindButton(string path, UnityEngine.Events.UnityAction callback)
    {
        var btn = transform.Find(path)?.GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(callback);
        else
            Debug.LogWarning($"[RT_MainMenuUI] 버튼을 찾을 수 없음: {path}");
        return btn;
    }
}
