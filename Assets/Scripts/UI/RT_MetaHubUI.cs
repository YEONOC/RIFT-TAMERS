using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 메타 허브 씬 UI 컨트롤러.
/// Canvas 에 부착하며, 자식 버튼에 onClick 이벤트를 바인딩한다.
/// </summary>
public class RT_MetaHubUI : MonoBehaviour
{
    private void Start()
    {
        BindButton("Btn_StartRun",              OnStartRunClicked);
        BindButton("Panel_Center/Btn_Compendium", OnCompendiumClicked);
        BindButton("Panel_Center/Btn_Bond",       OnBondClicked);
        BindButton("Panel_Center/Btn_SkillTree",  OnSkillTreeClicked);

        RefreshTamerInfo();
    }

    // ─────────────────────────────────────────
    // 버튼 콜백
    // ─────────────────────────────────────────

    private void OnStartRunClicked()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.StartNewRun();
    }

    private void OnCompendiumClicked()
    {
        // TODO: 크리처 도감 패널 열기 (Milestone 3 예정)
        Debug.Log("[MetaHubUI] 크리처 도감은 아직 구현되지 않았습니다.");
    }

    private void OnBondClicked()
    {
        // TODO: 유대 시스템 패널 열기 (Milestone 3 예정)
        Debug.Log("[MetaHubUI] 유대 시스템은 아직 구현되지 않았습니다.");
    }

    private void OnSkillTreeClicked()
    {
        // TODO: 스킬 트리 패널 열기 (Milestone 3 예정)
        Debug.Log("[MetaHubUI] 스킬 트리는 아직 구현되지 않았습니다.");
    }

    // ─────────────────────────────────────────
    // UI 갱신
    // ─────────────────────────────────────────

    /// <summary>상단 테이머 레벨/이름 텍스트 갱신</summary>
    private void RefreshTamerInfo()
    {
        if (GameManager.Instance == null) return;

        var textObj = transform.Find("Panel_TamerInfo/Text_TamerLevel");
        if (textObj == null) return;

        var text = textObj.GetComponent<Text>();
        if (text == null) return;

        int lv = GameManager.Instance.SaveData.tamerLevel;
        text.text = $"Lv.{lv} 테이머";
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
            Debug.LogWarning($"[MetaHubUI] 버튼을 찾을 수 없음: {path}");
    }
}
