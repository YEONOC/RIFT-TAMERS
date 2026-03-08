/// <summary>
/// 게임 전체에서 사용하는 열거형 정의
/// </summary>

/// <summary>크리처 등급</summary>
public enum CreatureGrade { C, B, A, S }

/// <summary>크리처 진영 (판타지 차원 / 현대 차원)</summary>
public enum CreatureFaction { Fantasy, Modern }

/// <summary>방 유형</summary>
public enum RoomType { Battle, Event, Shop, Rest, Treasure, CaptureChallenge, Boss }

/// <summary>상태이상 종류</summary>
public enum StatusEffectType { Burn, Freeze, Shock, Poison, Stun }

/// <summary>소울 젬 등급</summary>
public enum SoulGemGrade { Normal, Superior, Legendary }

/// <summary>게임 전체 상태</summary>
public enum GameState { MainMenu, MetaHub, InRun, Paused, GameOver }

/// <summary>스킬 타겟 종류</summary>
public enum SkillTargetType { SingleEnemy, AllEnemies, SingleAlly, AllAllies, Self }

/// <summary>스킬 피해 유형</summary>
public enum SkillDamageType { Physical, Magical, True }

/// <summary>테이머 스킬 슬롯</summary>
public enum TamerSkillSlot { Q, E, R }

/// <summary>Zone 테마</summary>
public enum ZoneTheme { FantasyStone, MixedRuins, RiftCore }
