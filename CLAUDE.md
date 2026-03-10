# RIFT TAMERS — CLAUDE.md

> Claude Code가 이 프로젝트에서 작업할 때의 최상위 가이드입니다.
> 작업 지시를 받을 때마다 이 문서를 먼저 숙지하고, 명시된 환경·설계 원칙을 따르십시오.

---

## 1. 프로젝트 개요

| 항목 | 내용 |
|------|------|
| 게임 제목 | RIFT TAMERS (리프트 테이머즈) |
| 장르 | 2D 도트 그래픽 로그라이크 |
| 엔진 | Unity 6000.3.10f1 |
| 렌더 파이프라인 | Universal Render Pipeline (URP) 17.3.0 — 2D Renderer |
| 타겟 플랫폼 | StandaloneWindows64 (Steam) |
| 개발 인원 | 1인 인디 |
| 그래픽 | AI 생성 도트 그래픽 (Aseprite) |
| 음악/SFX | AI 생성 |
| 코딩 보조 | Claude Code (MCP for Unity 연동) |

### 세계관 요약
차원의 균열(Rift)이 열리며 판타지 차원(마법, 도검, 중세)과 현대 차원(총, 로봇, 사이보그)의 크리처들이 뒤섞인 세계. 플레이어는 '테이머(Tamer)'로서 크리처들을 포획·육성해 Rift 던전을 탐험한다.

---

## 2. Unity 환경 설정

- **엔진 버전**: Unity 6000.3.10f1
- **렌더 파이프라인**: URP 17.3.0, 2D Renderer (`Assets/Settings/Renderer2D.asset`)
- **입력 시스템**: New Input System 1.18.0 — `Assets/InputSystem_Actions.inputactions` 에셋 사용
- **UI**: uGUI 2.0.0 (Canvas 기반)
- **아트 파이프라인**: Aseprite 임포터 (`.ase`/`.aseprite`), PSD 임포터 (레이어드 에셋)
- **애니메이션**: 2D Animation 패키지 (스켈레탈 리깅 지원)
- **타일맵**: `com.unity.2d.tilemap` + `com.unity.2d.tilemap.extras` (Rule Tile, Animated Tile)
- **씬 템플릿**: `Assets/Settings/Scenes/URP2DSceneTemplate.unity` — 신규 씬 생성 시 이 템플릿 사용

---

## 3. 핵심 패키지 목록

| 패키지 | 버전 | 용도 |
|--------|------|------|
| `com.unity.render-pipelines.universal` | 17.3.0 | URP 2D 렌더링 |
| `com.unity.inputsystem` | 1.18.0 | 플레이어 입력 |
| `com.unity.2d.animation` | 13.0.4 | 스켈레탈 2D 애니메이션 |
| `com.unity.2d.aseprite` | 3.0.1 | Aseprite 파일 직접 임포트 |
| `com.unity.2d.tilemap.extras` | 6.0.1 | Rule Tile, Animated Tile |
| `com.unity.2d.spriteshape` | 13.0.0 | 유기적 지형/경로 |
| `com.unity.timeline` | 1.8.10 | 컷씬 / 시퀀스 이벤트 |
| `com.unity.ugui` | 2.0.0 | Canvas UI |
| `com.unity.test-framework` | 1.6.0 | EditMode/PlayMode 테스트 |
| `com.coplaydev.unity-mcp` | git main | MCP for Unity (Claude Code 연동) |

---

## 4. MCP for Unity 워크플로우

Unity Editor가 실행 중이고 MCP 연결이 활성화되어 있어야 MCP 툴이 작동합니다.

### 표준 작업 순서
1. 에디터 상태 확인: `mcpforunity://editor/state` → `ready_for_tools: true` 확인
2. 스크립트 생성/수정 후: `refresh_unity` 호출 → `read_console`로 컴파일 에러 확인 (다음 작업 전 반드시 수행)
3. 여러 GameObject/컴포넌트 작업은 `batch_execute` 사용 (1회 최대 25개)
4. 시각적 결과 확인: `manage_scene(action="screenshot", include_image=True)`

### 테스트 실행
```
run_tests(mode="EditMode")   # 또는 "PlayMode"
get_test_job(job_id=..., wait_timeout=60, include_failed_tests=True)
```

---

## 5. 핵심 게임 루프

```
[런 시작]
    ↓
크리처 초기 선택 (3종 중 1종 또는 랜덤 2종 제공)
    ↓
던전 탐험 (Zone 1 → Zone 2 → Zone 3)
    ↓ 각 Floor마다
  [방 선택] → [전투 / 이벤트 / 상점 / 휴식]
    ↓ 전투 중
  [포획 시도] → 소울 젬 투척 → 성공 시 파티/보관함에 추가
    ↓
  [미니 보스 / 최종 보스]
    ↓
[런 종료] → 결과 계산 → 메타 성장 적용
    ↓
[다음 런] (점진적으로 강해짐)
```

---

## 6. 폴더 구조 및 아키텍처 원칙

### 6-1. 폴더 구조
```
Assets/
├── Scripts/
│   ├── Core/           # GameManager, SceneManager, EventBus, GameConstants
│   ├── Creature/       # CreatureData, CreatureAI, CreatureStats
│   ├── Player/         # TamerController, TamerSkills
│   ├── Combat/         # CombatSystem, DamageCalculator, StatusEffect
│   ├── Dungeon/        # DungeonGenerator, RoomController, FloorData
│   ├── Capture/        # CaptureSystem, SoulGem
│   ├── Meta/           # MetaProgressionSystem, BondSystem, TamerLevel
│   ├── UI/             # HUD, Inventory, DungeonMap, Menus
│   ├── Items/          # ItemData, ItemEffect, Shop
│   └── Utils/          # Extensions, Helpers, Constants
├── Data/               # ScriptableObject 에셋 (크리처, 던전, 아이템 데이터)
├── Prefabs/
│   ├── Creatures/
│   ├── Rooms/
│   ├── UI/
│   └── Effects/
├── Art/                # 스프라이트, Aseprite 파일 등 모든 시각 에셋
│   ├── Creatures/
│   ├── Tilesets/
│   ├── UI/
│   └── Effects/
├── Audio/              # BGM, SFX
├── Scenes/
│   ├── MainMenu
│   ├── Dungeon
│   └── MetaHub         # 런 사이 허브 화면
└── Settings/           # URP 설정, Input Actions 등 (기존 유지)
```

### 6-2. 핵심 패턴 & 규칙
- **ScriptableObject 기반 데이터**: 모든 크리처·아이템·방 데이터는 ScriptableObject로 정의. 코드에 하드코딩 금지.
- **EventBus 패턴**: 시스템 간 직접 참조 최소화. 이벤트로 통신 (`GameEvents.cs`에 모든 이벤트 상수 정의).
- **싱글톤은 Manager 클래스에만**: `GameManager`, `AudioManager`, `MetaProgressionManager`만 싱글톤 허용. 나머지는 의존성 주입.
- **비동기 처리**: 씬 전환·저장은 `async/await` 또는 코루틴 사용.
- **데이터 저장**: `JsonUtility` + `PlayerPrefs` 조합. 세이브 파일은 `Application.persistentDataPath` 하위에 저장.
- **입력 시스템**: `Input.GetKey()` 사용 금지 → New Input System (`InputAction` / `PlayerInput`)만 사용.
- **렌더링**: Built-in RP 셰이더 사용 금지 → URP 호환 셰이더/머티리얼만 사용.
- **ScriptableObject 파일명**: `[타입]_[이름]` 형식 (예: `Creature_GoblinWarrior`, `Room_BattleBasic`)

---

## 7. 핵심 시스템 상세 설계

### 7-1. 크리처 시스템 (Creature System)

#### CreatureData (ScriptableObject)
```csharp
[CreateAssetMenu(menuName = "RiftTamers/CreatureData")]
public class CreatureData : ScriptableObject
{
    [Header("기본 정보")]
    public string creatureId;       // 고유 ID (예: "goblin_warrior")
    public string displayName;      // 표시 이름
    public CreatureGrade grade;     // C / B / A / S
    public CreatureFaction faction; // Fantasy / Modern
    public Sprite[] idleSprites;    // 도트 애니메이션 프레임

    [Header("기본 스탯")]
    public float baseHP;
    public float baseATK;
    public float baseDEF;
    public float baseSpeed;
    public float captureBaseRate;   // 기본 포획률 (0.0 ~ 1.0)

    [Header("스킬")]
    public SkillData[] skills;      // 최대 3개

    [Header("시너지")]
    public string[] synergyTags;    // 예: ["드래곤족", "화염속성"]

    [Header("포획 조건")]
    public bool requiresSpecialCapture;
    public string specialCaptureCondition; // 보스 등 특수 조건 설명
}

public enum CreatureGrade { C, B, A, S }
public enum CreatureFaction { Fantasy, Modern }
```

#### 파티 구성
- 최대 파티 슬롯: **4칸** (기본 3, 메타 해금으로 4)
- 보관함: 런 중 최대 **8마리** 보관, 휴식방에서 교체 가능

#### 크리처 AI 행동 원칙
- 각 크리처는 `CreatureAI` 컴포넌트를 가짐
- AI 상태머신: `Idle → Move → Attack → Skill → Capture_Resist`
- 적 크리처 AI도 동일 구조 사용 (플레이어 측과 동일 베이스 클래스)
- 스킬 쿨다운·타겟 선택은 `CombatSystem`이 중앙 관리

---

### 7-2. 포획 시스템 (Capture System)

```
포획 성공률 계산:
  successRate = captureBaseRate × (1 + (1 - currentHP / maxHP) × 1.5)
  successRate = Clamp(successRate × gemBonusMultiplier, 0.05f, 0.95f)
```

- **소울 젬 등급**: 일반(×1.0), 상급(×1.5), 전설(×2.5)
- **포획 불가 상태**: 보스 크리처는 HP 10% 이하 + 해당 Zone 클리어 조건 충족 시만 가능
- **포획 실패**: 해당 크리처가 즉시 강화(ATK +10%) 후 반격 모드 진입
- 포획 성공 연출: 소울 젬 흡수 이펙트 → 파티/보관함 UI 팝업

---

### 7-3. 던전 생성 시스템 (Dungeon Generation)

#### 구조
```
Run = Zone 3개
Zone = Floor 5개
Floor = Room N개 (그래프 기반 분기)

Floor 구성 비율:
  - 전투방:      40%
  - 이벤트방:    15%
  - 상점:        10%
  - 휴식방:      15%
  - 보물방:      10%
  - 포획챌린지방: 10%
  (Floor 5는 항상 보스방)
```

#### 절차적 생성 규칙
- 각 Floor는 시작방 1개 + 분기(2~3갈래) + 합류 + 보스/엘리트방 구조
- 동일 방 유형이 3번 연속 배치되지 않도록 제한
- Zone별 테마: Zone1(판타지 석조), Zone2(혼합/폐허), Zone3(리프트 코어/우주적)
- 방 프리팹 Pool에서 랜덤 선택 + 시드 기반 재현 가능 (`RunSeed` 저장)

---

### 7-4. 전투 시스템 (Combat System)

#### 전투 흐름
1. 방 입장 → 적 크리처 스폰 (방 데이터 기반)
2. 실시간 진행: 플레이어 테이머 이동·회피 조작 + 크리처 자동 전투
3. 테이머 스킬 (Q/E/R): 쿨다운 기반 수동 발동
4. 모든 적 처치 또는 도주 시 전투 종료

#### 데미지 공식
```
finalDamage = (ATK × skillMultiplier) - (targetDEF × 0.5f)
finalDamage = Max(finalDamage, 1)
// 크리티컬: ×1.5, 속성 상성: ×1.2 또는 ×0.8
```

#### 상태이상 목록
| 상태 | 효과 | 최대 중첩 |
|------|------|-----------|
| 화상 | 매 초 HP 5% 감소 | 3 |
| 빙결 | 이동속도 -50% | 1 |
| 감전 | 스킬 쿨다운 +30% | 2 |
| 중독 | 매 초 HP 3% 감소 + DEF -20% | 1 |
| 기절 | 행동 불가 1.5초 | 1 |

---

### 7-5. 메타 성장 시스템 (Meta Progression)

#### 유대 시스템 (Bond System)
```csharp
public class BondData
{
    public string creatureId;
    public int bondPoints;    // 런 완주 +10, 보스 처치 참여 +5, 포획 성공 +3
    public int bondLevel;     // 1~5 (각 레벨 임계값: 10, 25, 50, 100, 200)
    public float[] statBonusPerLevel; // 레벨별 영구 스탯 보너스
}
```

#### 테이머 레벨
- 경험치 획득: 런 클리어(100), Zone 완주(30), 보스 처치(50), 크리처 도감 신규 등록(20)
- 레벨업 보상: 시작 파티 슬롯 해금, 테이머 패시브 스킬 선택, 소울 젬 초기 지급량 증가

#### 영구 해금 트리
```
[던전 해금]
  └ Zone2 해금 (기본 제공)
  └ Zone3 해금 (Zone2 보스 최초 처치)
  └ 특수 던전: "분열의 리프트" (조건: S급 크리처 3종 도감 등록)

[크리처 도감]
  └ 등록된 크리처는 이후 런에서 출현률 +5%

[테이머 스킬 트리]
  └ 포획 강화 계열 / 지원 강화 계열 / 생존 강화 계열 (3갈래)
```

---

## 8. 테이머 스킬 목록

| 슬롯 | 스킬명 | 효과 | 쿨다운 |
|------|--------|------|--------|
| Q | 소울 젬 투척 | 포획 시도 | 기본 5초 |
| E | 전투 포효 | 아군 크리처 ATK +30% (5초) | 15초 |
| R | 비상 회수 | 크리처 1마리를 보관함으로 즉시 회수 + 해당 크리처 HP 50% 회복 | 30초 |
| Passive | 테이머의 유대 | 유대 레벨 1 이상 크리처 스탯 +5% | 상시 |

---

## 9. UI/UX 가이드라인

- **해상도**: 기본 1920×1080, 16:9 기준 설계
- **도트 그래픽 스케일**: 픽셀 퍼 유닛 16 또는 32 통일
- **카메라**: 픽셀 퍼펙트 카메라 컴포넌트 사용 (`UnityEngine.U2D`)
- **HUD 구성**: 좌하단 파티 크리처 HP 바 / 우하단 테이머 스킬 아이콘 / 상단 중앙 Floor 정보
- **폰트**: 도트/픽셀 폰트 사용 (예: DotGothic16 또는 커스텀 폰트)
- **색상 팔레트**: 각 Zone별 일관된 팔레트 적용 (ScriptableObject로 관리)

---

## 10. 데이터 저장 구조

```csharp
[Serializable]
public class SaveData
{
    // 메타 데이터 (영구 보존)
    public int tamerLevel;
    public int tamerExp;
    public List<BondData> bondDataList;
    public List<string> unlockedCreatureIds;  // 도감
    public List<string> unlockedDungeons;
    public int tamerSkillPoints;
    public int[] tamerSkillTree;              // 비트마스크

    // 현재 런 데이터 (런 중에만 유효)
    public bool isRunActive;
    public int currentZone;
    public int currentFloor;
    public long runSeed;
    public List<string> partyCreatureIds;
    public List<string> storageCreatureIds;
    public int soulGemCount;
    public List<string> itemIds;
}
```

---

## 11. 개발 우선순위 (마일스톤)

### Milestone 1 — 프로토타입 (핵심 루프 검증)
- [v] 기본 씬 구성 (MainMenu, Dungeon, MetaHub)
- [v] TamerController (이동, 회피, Q 스킬)
- [ ] 크리처 1~3종 구현 (CreatureData + CreatureAI)
- [ ] 단순 전투방 1종 구현
- [ ] 기본 포획 시스템 (CaptureSystem)
- [ ] 런 시작/종료 흐름

### Milestone 2 — 코어 시스템 완성
- [ ] 던전 절차적 생성 (DungeonGenerator)
- [ ] 방 유형 전체 구현
- [ ] 크리처 10종 이상 구현
- [ ] Zone 1 전체 (보스 포함)
- [ ] 기본 메타 성장 (BondSystem, TamerLevel)
- [ ] 저장/불러오기

### Milestone 3 — 콘텐츠 확장
- [ ] Zone 2, 3 구현
- [ ] 크리처 30종 이상
- [ ] 아이템 시스템 + 상점
- [ ] 이벤트방 이벤트 20종 이상
- [ ] 테이머 스킬 트리 전체
- [ ] 영구 해금 트리 전체

### Milestone 4 — 폴리싱 & 출시 준비
- [ ] 도트 그래픽 에셋 전체 교체
- [ ] AI 생성 BGM/SFX 적용
- [ ] 밸런싱 패스
- [ ] Steam 연동 (업적, 리더보드)
- [ ] 빌드 최적화 및 QA

---

## 12. 코딩 컨벤션

- **언어**: C# (Unity 스타일)
- **명명 규칙**:
  - 클래스/프리팹: PascalCase (`CreatureAI`, `DungeonGenerator`)
  - 변수/함수: camelCase (`currentHP`, `TakeDamage()`)
  - 상수: UPPER_SNAKE_CASE (`MAX_PARTY_SLOTS`)
  - ScriptableObject 파일명: `[타입]_[이름]` (예: `Creature_GoblinWarrior`)
- **주석**: 공개 메서드·클래스에 XML 문서 주석 필수
- **경고 처리**: Unity 경고는 방치하지 않고 즉시 해결
- **테스트**: 핵심 시스템(포획 공식, 데미지 계산)은 Unity Test Framework로 단위 테스트 작성

---

## 13. Claude Code 작업 지침

Claude Code가 작업을 받을 때 반드시 따를 원칙:

1. **이 문서 우선**: 모든 구현은 위 설계를 기반으로 함. 설계와 충돌하는 구현 요청이 있으면 먼저 확인을 요청할 것.
2. **ScriptableObject 우선**: 수치·설정 데이터는 절대 코드에 하드코딩하지 않음.
3. **기능 단위 작업**: 한 번에 하나의 시스템을 완성하고 테스트 후 다음으로 진행.
4. **기존 코드 존중**: 새 기능 추가 시 기존 아키텍처를 파악하고 일관성 유지.
5. **파일 위치 준수**: 위 폴더 구조를 엄수. 임의로 폴더를 만들지 않음.
6. **한국어 주석**: 모든 주석 및 Inspector 표시 문자열은 한국어로 작성.
7. **밸런스 수치 분리**: 게임 밸런스 관련 수치는 `GameConstants.cs` 또는 ScriptableObject에 집중 관리.
8. **파일 명 규칙**: 생성할 파일의 이름 앞에 RT_를 붙일 것.
9. **최적화 고려한 코드 작성**: 최적화를 고려해서 코드 작성하기.

---

*문서 최초 작성: 2026-03-10*
*버전: v1.2 (Unity 환경 설정 + 게임 설계 통합본)*