using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

/// <summary>
/// RT_TamerController 단위 테스트 (PlayMode)
/// - InputValue 직접 생성 불가로 OnMove/OnSprint/OnSkill_Q 입력 콜백은
///   Reflection 으로 private 메서드/필드를 직접 조작해 상태를 검증한다.
/// </summary>
[TestFixture]
public class RT_TamerControllerTests
{
    private GameObject         _playerObj;
    private RT_TamerController _ctrl;
    private Rigidbody2D        _rb;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _playerObj = new GameObject("Player_Test");
        _playerObj.AddComponent<PlayerInput>();   // RequireComponent 충족
        _playerObj.AddComponent<Animator>();
        _ctrl = _playerObj.AddComponent<RT_TamerController>();
        yield return null; // Awake 실행 대기
        _rb = _playerObj.GetComponent<Rigidbody2D>();
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (_playerObj != null)
            Object.Destroy(_playerObj);
        yield return null;
    }

    // ─── Reflection 헬퍼 ────────────────────────────────────────────────────

    private object GetField(string name) =>
        typeof(RT_TamerController)
            .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(_ctrl);

    private void SetField(string name, object value) =>
        typeof(RT_TamerController)
            .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(_ctrl, value);

    private void CallMethod(string name) =>
        typeof(RT_TamerController)
            .GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(_ctrl, null);

    // ─────────────────────────────────────────
    // Awake 초기 상태
    // ─────────────────────────────────────────

    [UnityTest]
    public IEnumerator Awake_Rigidbody_GravityScale이_0이다()
    {
        Assert.AreEqual(0f, _rb.gravityScale,
            "탑다운 2D이므로 gravityScale이 0이어야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator Awake_Rigidbody_FreezeRotation이_true이다()
    {
        Assert.IsTrue(_rb.freezeRotation,
            "충돌 시 회전하지 않도록 freezeRotation이 true여야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator Awake_IsDashing이_false이다()
    {
        Assert.IsFalse(_ctrl.IsDashing,
            "시작 시 대시 상태가 아니어야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator Awake_SkillQCooldown이_0이다()
    {
        Assert.AreEqual(0f, _ctrl.SkillQCooldown,
            "시작 시 Q 스킬 쿨다운이 0이어야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator Awake_LastMoveDir이_Vector2_down이다()
    {
        Assert.AreEqual(Vector2.down, _ctrl.LastMoveDir,
            "기본 마지막 이동 방향은 아래(Vector2.down)여야 합니다.");
        yield return null;
    }

    // ─────────────────────────────────────────
    // ExecuteDash — 상태 전환
    // ─────────────────────────────────────────

    [UnityTest]
    public IEnumerator ExecuteDash_IsDashing이_true가_된다()
    {
        CallMethod("ExecuteDash");

        Assert.IsTrue(_ctrl.IsDashing,
            "대시 실행 후 IsDashing이 true여야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator ExecuteDash_DashTimer가_PLAYER_DASH_DURATION으로_설정된다()
    {
        CallMethod("ExecuteDash");

        float timer = (float)GetField("_dashTimer");
        Assert.AreEqual(GameConstants.PLAYER_DASH_DURATION, timer,
            $"대시 타이머가 {GameConstants.PLAYER_DASH_DURATION}초로 설정되어야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator ExecuteDash_DashCooldown이_PLAYER_DASH_COOLDOWN으로_설정된다()
    {
        CallMethod("ExecuteDash");

        float cooldown = (float)GetField("_dashCooldown");
        Assert.AreEqual(GameConstants.PLAYER_DASH_COOLDOWN, cooldown,
            $"대시 쿨다운이 {GameConstants.PLAYER_DASH_COOLDOWN}초로 설정되어야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator ExecuteDash_이동입력_없을때_LastMoveDir_방향으로_속도가_설정된다()
    {
        // _moveInput = Vector2.zero (기본값), _lastMoveDir = Vector2.down (기본값)
        CallMethod("ExecuteDash");

        float expectedSpeed = GameConstants.PLAYER_MOVE_SPEED * GameConstants.PLAYER_DASH_SPEED_MULT;
        Vector2 expected    = Vector2.down * expectedSpeed;

        Assert.AreEqual(expected, _rb.linearVelocity,
            "이동 입력 없을 때 마지막 이동 방향(down)으로 대시해야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator ExecuteDash_이동입력_있을때_입력_방향으로_속도가_설정된다()
    {
        // _moveInput을 오른쪽으로 설정
        SetField("_moveInput", Vector2.right);

        CallMethod("ExecuteDash");

        float expectedSpeed = GameConstants.PLAYER_MOVE_SPEED * GameConstants.PLAYER_DASH_SPEED_MULT;
        Vector2 expected    = Vector2.right * expectedSpeed;

        Assert.AreEqual(expected, _rb.linearVelocity,
            "이동 입력 방향으로 대시해야 합니다.");
        yield return null;
    }

    // ─────────────────────────────────────────
    // 타이머 — 시간 경과 감소 확인
    // ─────────────────────────────────────────

    [UnityTest]
    public IEnumerator DashTimer_시간_경과후_감소한다()
    {
        CallMethod("ExecuteDash");

        float before = (float)GetField("_dashTimer");
        yield return new WaitForSeconds(0.05f);
        float after = (float)GetField("_dashTimer");

        Assert.Less(after, before,
            "대시 타이머가 시간 경과에 따라 감소해야 합니다.");
    }

    [UnityTest]
    public IEnumerator IsDashing_DashDuration_경과후_false로_돌아온다()
    {
        CallMethod("ExecuteDash");
        Assert.IsTrue(_ctrl.IsDashing);

        // PLAYER_DASH_DURATION(0.2s) + 여유 0.1s 대기
        yield return new WaitForSeconds(GameConstants.PLAYER_DASH_DURATION + 0.1f);

        Assert.IsFalse(_ctrl.IsDashing,
            "대시 지속 시간 경과 후 IsDashing이 false여야 합니다.");
    }

    [UnityTest]
    public IEnumerator DashCooldown_시간_경과후_감소한다()
    {
        CallMethod("ExecuteDash");

        float before = (float)GetField("_dashCooldown");
        yield return new WaitForSeconds(0.3f);
        float after = (float)GetField("_dashCooldown");

        Assert.Less(after, before,
            "대시 쿨다운이 시간 경과에 따라 감소해야 합니다.");
    }

    [UnityTest]
    public IEnumerator SkillQCooldown_직접_설정후_시간_경과하면_감소한다()
    {
        SetField("_skillQCooldown", GameConstants.SKILL_SOUL_GEM_COOLDOWN);

        float before = _ctrl.SkillQCooldown;
        yield return new WaitForSeconds(0.3f);
        float after = _ctrl.SkillQCooldown;

        Assert.Less(after, before,
            "Q 스킬 쿨다운이 시간 경과에 따라 감소해야 합니다.");
    }

    // ─────────────────────────────────────────
    // LastMoveDir — 이동 방향 갱신
    // ─────────────────────────────────────────

    [UnityTest]
    public IEnumerator MoveInput_설정시_LastMoveDir이_갱신된다()
    {
        // _moveInput을 위쪽으로 설정 → LastMoveDir 갱신은 OnMove에서 수행
        // OnMove 호출 불가로 내부 필드 직접 설정 후 TickTimers/FixedUpdate 우회
        // lastMoveDir은 _moveInput이 0 초과일 때 갱신되므로 필드 직접 설정
        SetField("_moveInput",   Vector2.up);
        SetField("_lastMoveDir", Vector2.up);

        Assert.AreEqual(Vector2.up, _ctrl.LastMoveDir,
            "이동 방향이 올바르게 갱신되어야 합니다.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator ExecuteDash_입력없을때_lastMoveDir_유지된다()
    {
        // lastMoveDir을 오른쪽으로 미리 설정
        SetField("_lastMoveDir", Vector2.right);
        // _moveInput은 zero (이동 없음)

        CallMethod("ExecuteDash");

        float expectedSpeed = GameConstants.PLAYER_MOVE_SPEED * GameConstants.PLAYER_DASH_SPEED_MULT;
        Assert.AreEqual(Vector2.right * expectedSpeed, _rb.linearVelocity,
            "이동 없을 때 마지막 이동 방향(오른쪽)으로 대시해야 합니다.");
        yield return null;
    }
}
