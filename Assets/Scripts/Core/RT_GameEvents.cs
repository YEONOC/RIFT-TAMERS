using System;

/// <summary>
/// 게임 전체 이벤트 버스
/// 시스템 간 직접 참조 대신 이벤트로 통신한다.
/// 씬 전환 전 ClearAllListeners() 를 호출해 메모리 누수를 방지할 것.
/// </summary>
public static class GameEvents
{
    // ─── 전투 이벤트 ──────────────────────────────────────────────────────

    /// <summary>크리처가 피해를 받을 때 (creatureId, 피해량)</summary>
    public static event Action<string, float> OnCreatureTakeDamage;

    /// <summary>크리처 사망 시 (creatureId)</summary>
    public static event Action<string> OnCreatureDied;

    /// <summary>포획 시도 결과 (creatureId, 성공 여부)</summary>
    public static event Action<string, bool> OnCaptureAttempted;

    // ─── 런 이벤트 ────────────────────────────────────────────────────────

    /// <summary>런 시작</summary>
    public static event Action OnRunStarted;

    /// <summary>런 종료 (true = 클리어, false = 사망)</summary>
    public static event Action<bool> OnRunEnded;

    /// <summary>Floor 이동 시 (zone 번호, floor 번호)</summary>
    public static event Action<int, int> OnFloorChanged;

    // ─── UI / 파티 이벤트 ────────────────────────────────────────────────

    /// <summary>파티 구성이 변경될 때</summary>
    public static event Action OnPartyChanged;

    /// <summary>소울 젬 수량 변경 (현재 보유량)</summary>
    public static event Action<int> OnSoulGemCountChanged;

    // ─── 메타 이벤트 ──────────────────────────────────────────────────────

    /// <summary>테이머 레벨업 (새 레벨)</summary>
    public static event Action<int> OnTamerLevelUp;

    /// <summary>유대 레벨업 (creatureId, 새 유대 레벨)</summary>
    public static event Action<string, int> OnBondLevelUp;

    /// <summary>도감에 신규 크리처 등록 (creatureId)</summary>
    public static event Action<string> OnCreatureRegisteredToCodex;

    // ─── 이벤트 발행 메서드 ───────────────────────────────────────────────

    public static void RaiseCreatureTakeDamage(string creatureId, float damage)
        => OnCreatureTakeDamage?.Invoke(creatureId, damage);

    public static void RaiseCreatureDied(string creatureId)
        => OnCreatureDied?.Invoke(creatureId);

    public static void RaiseCaptureAttempted(string creatureId, bool success)
        => OnCaptureAttempted?.Invoke(creatureId, success);

    public static void RaiseRunStarted()
        => OnRunStarted?.Invoke();

    public static void RaiseRunEnded(bool isCleared)
        => OnRunEnded?.Invoke(isCleared);

    public static void RaiseFloorChanged(int zone, int floor)
        => OnFloorChanged?.Invoke(zone, floor);

    public static void RaisePartyChanged()
        => OnPartyChanged?.Invoke();

    public static void RaiseSoulGemCountChanged(int count)
        => OnSoulGemCountChanged?.Invoke(count);

    public static void RaiseTamerLevelUp(int newLevel)
        => OnTamerLevelUp?.Invoke(newLevel);

    public static void RaiseBondLevelUp(string creatureId, int newLevel)
        => OnBondLevelUp?.Invoke(creatureId, newLevel);

    public static void RaiseCreatureRegisteredToCodex(string creatureId)
        => OnCreatureRegisteredToCodex?.Invoke(creatureId);

    /// <summary>
    /// 모든 이벤트 리스너 제거 — 씬 전환 시 호출
    /// </summary>
    public static void ClearAllListeners()
    {
        OnCreatureTakeDamage        = null;
        OnCreatureDied              = null;
        OnCaptureAttempted          = null;
        OnRunStarted                = null;
        OnRunEnded                  = null;
        OnFloorChanged              = null;
        OnPartyChanged              = null;
        OnSoulGemCountChanged       = null;
        OnTamerLevelUp              = null;
        OnBondLevelUp               = null;
        OnCreatureRegisteredToCodex = null;
    }
}
