using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 테이머(플레이어) 이동·대시·Q 스킬을 처리하는 컨트롤러.
/// PlayerInput 컴포넌트의 "Send Messages" 모드로 입력을 수신한다.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class RT_TamerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator    _animator;

    private Vector2 _moveInput;
    private Vector2 _lastMoveDir = Vector2.down; // 마지막 이동 방향 (대시 방향 결정용)

    // ─── 대시 ───────────────────────────────────────────────────────────────
    private bool  _isDashing;
    private float _dashTimer;    // 대시 지속 타이머
    private float _dashCooldown; // 대시 쿨다운 타이머

    // ─── Q 스킬 ─────────────────────────────────────────────────────────────
    private float _skillQCooldown;

    // ─────────────────────────────────────────
    // 초기화
    // ─────────────────────────────────────────

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _rb.gravityScale  = 0f;  // 탑다운 2D: 중력 없음
        _rb.freezeRotation = true;
    }

    // ─────────────────────────────────────────
    // 업데이트
    // ─────────────────────────────────────────

    private void Update()
    {
        TickTimers();
    }

    private void FixedUpdate()
    {
        if (_isDashing) return; // 대시 중 이동 입력 무시

        _rb.linearVelocity = _moveInput * GameConstants.PLAYER_MOVE_SPEED;
        UpdateAnimation();
    }

    private void TickTimers()
    {
        if (_dashTimer > 0)
        {
            _dashTimer -= Time.deltaTime;
            if (_dashTimer <= 0)
                _isDashing = false;
        }

        if (_dashCooldown  > 0) _dashCooldown  -= Time.deltaTime;
        if (_skillQCooldown > 0) _skillQCooldown -= Time.deltaTime;
    }

    // ─────────────────────────────────────────
    // Input 콜백 (PlayerInput → Send Messages)
    // ─────────────────────────────────────────

    /// <summary>이동 입력 (WASD / 방향키 / 게임패드 좌스틱)</summary>
    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        if (_moveInput.sqrMagnitude > 0.01f)
            _lastMoveDir = _moveInput.normalized;
    }

    /// <summary>대시 입력 (Left Shift / 게임패드 L3)</summary>
    public void OnSprint(InputValue value)
    {
        if (value.isPressed && !_isDashing && _dashCooldown <= 0)
            ExecuteDash();
    }

    /// <summary>Q 스킬 — 소울 젬 투척 (Left Shift / 게임패드 R1)</summary>
    public void OnSkill_Q(InputValue value)
    {
        if (value.isPressed && _skillQCooldown <= 0)
        {
            _skillQCooldown = GameConstants.SKILL_SOUL_GEM_COOLDOWN;
            // TODO: 포획 시스템 연결 (Milestone 1-3)
            Debug.Log("[TamerController] Q 스킬: 소울 젬 투척 — 쿨다운 시작");
        }
    }

    // ─────────────────────────────────────────
    // 대시
    // ─────────────────────────────────────────

    private void ExecuteDash()
    {
        _isDashing    = true;
        _dashTimer    = GameConstants.PLAYER_DASH_DURATION;
        _dashCooldown = GameConstants.PLAYER_DASH_COOLDOWN;

        Vector2 dir = _moveInput.sqrMagnitude > 0.01f
            ? _moveInput.normalized
            : _lastMoveDir;

        _rb.linearVelocity = dir * GameConstants.PLAYER_MOVE_SPEED * GameConstants.PLAYER_DASH_SPEED_MULT;
    }

    // ─────────────────────────────────────────
    // 애니메이션
    // ─────────────────────────────────────────

    private void UpdateAnimation()
    {
        if (_animator == null) return;

        bool isMoving = _moveInput.sqrMagnitude > 0.01f;
        _animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            _animator.SetFloat("MoveX", _moveInput.x);
            _animator.SetFloat("MoveY", _moveInput.y);
        }
    }

    // ─────────────────────────────────────────
    // 공개 프로퍼티
    // ─────────────────────────────────────────

    /// <summary>현재 대시 중인지 여부 (무적 판정 등 외부 참조용)</summary>
    public bool IsDashing => _isDashing;

    /// <summary>Q 스킬 남은 쿨다운 (HUD 표시용)</summary>
    public float SkillQCooldown => _skillQCooldown;

    /// <summary>마지막 이동 방향 (크리처 AI 등 외부 참조용)</summary>
    public Vector2 LastMoveDir => _lastMoveDir;
}
