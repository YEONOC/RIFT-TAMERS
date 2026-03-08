/// <summary>
/// 게임 전체 밸런스 수치 상수 모음
/// 모든 수치 수정은 이 파일 또는 ScriptableObject에서만 수행
/// </summary>
public static class GameConstants
{
    // ─── 파티 / 보관함 ──────────────────────────────────────────────────
    public const int MAX_PARTY_SLOTS_DEFAULT  = 3;
    public const int MAX_PARTY_SLOTS_UNLOCKED = 4;
    public const int MAX_STORAGE_SLOTS        = 8;

    // ─── 던전 구조 ───────────────────────────────────────────────────────
    public const int ZONES_PER_RUN  = 3;
    public const int FLOORS_PER_ZONE = 5;

    // ─── 방 구성 비율 (퍼센트, 합계 100) ─────────────────────────────────
    public const int ROOM_CHANCE_BATTLE            = 40;
    public const int ROOM_CHANCE_EVENT             = 15;
    public const int ROOM_CHANCE_SHOP              = 10;
    public const int ROOM_CHANCE_REST              = 15;
    public const int ROOM_CHANCE_TREASURE          = 10;
    public const int ROOM_CHANCE_CAPTURE_CHALLENGE = 10;

    // ─── 전투 / 데미지 ──────────────────────────────────────────────────
    public const float DAMAGE_DEF_MULTIPLIER        = 0.5f;
    public const float DAMAGE_MIN                   = 1f;
    public const float CRIT_MULTIPLIER              = 1.5f;
    public const float TYPE_ADVANTAGE_MULTIPLIER    = 1.2f;
    public const float TYPE_DISADVANTAGE_MULTIPLIER = 0.8f;

    // ─── 포획 ───────────────────────────────────────────────────────────
    public const float CAPTURE_HP_BONUS_MULTIPLIER = 1.5f;
    public const float CAPTURE_MIN_RATE            = 0.05f;
    public const float CAPTURE_MAX_RATE            = 0.95f;
    public const float CAPTURE_FAIL_ATK_BUFF       = 1.1f;  // 포획 실패 시 적 ATK ×1.1
    public const float BOSS_CAPTURE_HP_THRESHOLD   = 0.1f;  // 보스 포획 가능 HP 임계값

    // ─── 소울 젬 배율 ────────────────────────────────────────────────────
    public const float GEM_NORMAL_MULTIPLIER    = 1.0f;
    public const float GEM_SUPERIOR_MULTIPLIER  = 1.5f;
    public const float GEM_LEGENDARY_MULTIPLIER = 2.5f;

    // ─── 메타 경험치 ─────────────────────────────────────────────────────
    public const int EXP_RUN_CLEAR             = 100;
    public const int EXP_ZONE_CLEAR            = 30;
    public const int EXP_BOSS_KILL             = 50;
    public const int EXP_NEW_CREATURE_REGISTER = 20;

    // ─── 유대 포인트 ─────────────────────────────────────────────────────
    public const int BOND_POINTS_RUN_COMPLETE    = 10;
    public const int BOND_POINTS_BOSS_KILL       = 5;
    public const int BOND_POINTS_CAPTURE_SUCCESS = 3;

    // ─── 유대 레벨 임계값 (인덱스 = 레벨) ─────────────────────────────────
    public static readonly int[] BOND_LEVEL_THRESHOLDS = { 0, 10, 25, 50, 100, 200 };
    public const int   MAX_BOND_LEVEL            = 5;
    public const float BOND_STAT_BONUS_PER_LEVEL = 0.05f;

    // ─── 테이머 스킬 쿨다운 (초) ──────────────────────────────────────────
    public const float SKILL_SOUL_GEM_COOLDOWN         = 5f;
    public const float SKILL_BATTLE_ROAR_COOLDOWN      = 15f;
    public const float SKILL_EMERGENCY_RECALL_COOLDOWN = 30f;

    // ─── 테이머 스킬 효과 수치 ────────────────────────────────────────────
    public const float BATTLE_ROAR_ATK_BONUS       = 0.3f;
    public const float BATTLE_ROAR_DURATION        = 5f;
    public const float EMERGENCY_RECALL_HP_RESTORE = 0.5f;

    // ─── 상태이상 효과 ────────────────────────────────────────────────────
    public const float STATUS_BURN_DPS_RATE        = 0.05f;
    public const float STATUS_FREEZE_SPEED_PENALTY = 0.5f;
    public const float STATUS_SHOCK_CD_PENALTY     = 0.3f;
    public const float STATUS_POISON_DPS_RATE      = 0.03f;
    public const float STATUS_POISON_DEF_PENALTY   = 0.2f;
    public const float STATUS_STUN_DURATION        = 1.5f;

    // ─── 플레이어 이동 ────────────────────────────────────────────────────────
    public const float PLAYER_MOVE_SPEED      = 5f;
    public const float PLAYER_DASH_SPEED_MULT = 3f;
    public const float PLAYER_DASH_DURATION   = 0.2f;
    public const float PLAYER_DASH_COOLDOWN   = 1f;

    // ─── 카메라 추적 ─────────────────────────────────────────────────────────
    public const float CAMERA_FOLLOW_SMOOTH   = 5f;

    // ─── 비주얼 / 카메라 ──────────────────────────────────────────────────
    public const int PIXELS_PER_UNIT = 16;

    // ─── 도감 출현률 보정 ─────────────────────────────────────────────────
    public const float CREATURE_CODEX_SPAWN_BONUS = 0.05f;
}
