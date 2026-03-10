using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// GameManager 싱글톤·상태 변경 단위 테스트 (PlayMode)
/// 씬 전환 코루틴은 StopAllCoroutines()로 즉시 중단해 씬 전환을 막는다.
/// </summary>
[TestFixture]
public class RT_GameManagerTests
{
    private GameObject _gmObj;
    private RT_GameManager _gm;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _gmObj = new GameObject("GameManager_Test");
        _gm    = _gmObj.AddComponent<RT_GameManager>();
        yield return null; // Awake 실행 대기
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        GameEvents.ClearAllListeners();
        _gm.StopAllCoroutines(); // 씬 전환 코루틴 중단
        if (_gmObj != null)
            Object.Destroy(_gmObj);
        yield return null;
    }

    // ─────────────────────────────────────────
    // 싱글톤 테스트
    // ─────────────────────────────────────────

    [UnityTest]
    public IEnumerator Awake_Instance가_자기자신으로_설정된다()
    {
        Assert.AreEqual(_gm, RT_GameManager.Instance, "Instance가 생성된 GameManager여야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator 중복_GameManager_추가시_즉시_제거된다()
    {
        var duplicateObj = new GameObject("GameManager_Duplicate");
        duplicateObj.AddComponent<RT_GameManager>();
        yield return null; // Awake 실행

        Assert.IsTrue(duplicateObj == null,
            "중복 GameManager의 GameObject가 제거되지 않았습니다.");
        Assert.AreEqual(_gm, RT_GameManager.Instance,
            "원본 Instance가 유지되어야 합니다.");
    }

    // ─────────────────────────────────────────
    // StartNewRun 테스트
    // ─────────────────────────────────────────

    [UnityTest]
    public IEnumerator StartNewRun_isRunActive가_true가_된다()
    {
        _gm.StartNewRun();
        _gm.StopAllCoroutines(); // 씬 전환 방지

        Assert.IsTrue(_gm.SaveData.isRunActive,
            "StartNewRun 후 isRunActive가 true여야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator StartNewRun_currentZone이_1로_설정된다()
    {
        _gm.StartNewRun();
        _gm.StopAllCoroutines();

        Assert.AreEqual(1, _gm.SaveData.currentZone,
            "StartNewRun 후 currentZone이 1이어야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator StartNewRun_currentFloor가_1로_설정된다()
    {
        _gm.StartNewRun();
        _gm.StopAllCoroutines();

        Assert.AreEqual(1, _gm.SaveData.currentFloor,
            "StartNewRun 후 currentFloor가 1이어야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator StartNewRun_runSeed가_0이_아니다()
    {
        _gm.StartNewRun();
        _gm.StopAllCoroutines();

        Assert.AreNotEqual(0L, _gm.SaveData.runSeed,
            "StartNewRun 후 runSeed가 설정되어야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator StartNewRun_GameState가_InRun으로_변경된다()
    {
        _gm.StartNewRun();
        _gm.StopAllCoroutines();

        Assert.AreEqual(GameState.InRun, _gm.CurrentState,
            "StartNewRun 후 상태가 InRun이어야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator StartNewRun_OnRunStarted_이벤트가_발행된다()
    {
        bool eventFired = false;
        GameEvents.OnRunStarted += () => eventFired = true;

        _gm.StartNewRun();
        _gm.StopAllCoroutines();

        Assert.IsTrue(eventFired,
            "StartNewRun 시 OnRunStarted 이벤트가 발행되어야 합니다.");
        yield return null;
    }

    // ─────────────────────────────────────────
    // EndRun 테스트
    // ─────────────────────────────────────────

    [UnityTest]
    public IEnumerator EndRun_isRunActive가_false가_된다()
    {
        _gm.StartNewRun();
        _gm.StopAllCoroutines();

        _gm.EndRun(isCleared: true);
        _gm.StopAllCoroutines();

        Assert.IsFalse(_gm.SaveData.isRunActive,
            "EndRun 후 isRunActive가 false여야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator EndRun_GameState가_MetaHub로_변경된다()
    {
        _gm.StartNewRun();
        _gm.StopAllCoroutines();

        _gm.EndRun(isCleared: true);
        _gm.StopAllCoroutines();

        Assert.AreEqual(GameState.MetaHub, _gm.CurrentState,
            "EndRun 후 상태가 MetaHub여야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator EndRun_클리어_OnRunEnded_true로_발행된다()
    {
        // StartNewRun 은 코루틴 내부에서 ClearAllListeners() 를 호출한다.
        // 따라서 OnRunEnded 구독은 StartNewRun + StopAllCoroutines 이후에 해야 한다.
        _gm.StartNewRun();
        _gm.StopAllCoroutines();

        bool? cleared = null;
        GameEvents.OnRunEnded += isCleared => cleared = isCleared;

        _gm.EndRun(isCleared: true);
        _gm.StopAllCoroutines();

        Assert.IsTrue(cleared.HasValue && cleared.Value,
            "EndRun(isCleared:true) 시 OnRunEnded가 true로 발행되어야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator EndRun_실패_OnRunEnded_false로_발행된다()
    {
        _gm.StartNewRun();
        _gm.StopAllCoroutines();

        bool? cleared = null;
        GameEvents.OnRunEnded += isCleared => cleared = isCleared;

        _gm.EndRun(isCleared: false);
        _gm.StopAllCoroutines();

        Assert.IsTrue(cleared.HasValue && !cleared.Value,
            "EndRun(isCleared:false) 시 OnRunEnded가 false로 발행되어야 합니다.");
        yield return null;
    }

    // ─────────────────────────────────────────
    // DeleteSave 테스트
    // ─────────────────────────────────────────

    [UnityTest]
    public IEnumerator DeleteSave_isRunActive가_false로_초기화된다()
    {
        _gm.StartNewRun();
        _gm.StopAllCoroutines();
        _gm.DeleteSave();

        Assert.IsFalse(_gm.SaveData.isRunActive,
            "DeleteSave 후 isRunActive가 false여야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator DeleteSave_tamerLevel이_1로_초기화된다()
    {
        _gm.StartNewRun();
        _gm.StopAllCoroutines();
        _gm.DeleteSave();

        Assert.AreEqual(1, _gm.SaveData.tamerLevel,
            "DeleteSave 후 tamerLevel이 기본값 1이어야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator DeleteSave_SaveData가_null이_아니다()
    {
        _gm.DeleteSave();

        Assert.IsNotNull(_gm.SaveData,
            "DeleteSave 후 SaveData가 새로 생성되어야 합니다.");
        yield return null;
    }
}
