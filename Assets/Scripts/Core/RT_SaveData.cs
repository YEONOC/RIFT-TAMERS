using System;
using System.Collections.Generic;

/// <summary>
/// 게임 전체 세이브 데이터 구조
/// 저장 경로: Application.persistentDataPath/save.json
/// GameManager 가 JsonUtility 로 직렬화/역직렬화
/// </summary>
[Serializable]
public class SaveData
{
    // ─── 메타 데이터 (런과 무관하게 영구 보존) ──────────────────────────

    /// <summary>테이머 레벨</summary>
    public int tamerLevel = 1;

    /// <summary>테이머 경험치</summary>
    public int tamerExp = 0;

    /// <summary>크리처별 유대 데이터 목록</summary>
    public List<BondData> bondDataList = new List<BondData>();

    /// <summary>도감에 등록된 크리처 ID 목록</summary>
    public List<string> unlockedCreatureIds = new List<string>();

    /// <summary>해금된 던전 ID 목록 (Zone1은 기본 해금)</summary>
    public List<string> unlockedDungeons = new List<string> { "dungeon_zone1" };

    /// <summary>남은 테이머 스킬 포인트</summary>
    public int tamerSkillPoints = 0;

    /// <summary>테이머 스킬 트리 습득 상태 (비트마스크)</summary>
    public int[] tamerSkillTree = new int[0];

    // ─── 현재 런 데이터 (런 중에만 유효) ──────────────────────────────

    /// <summary>현재 런이 진행 중인지 여부</summary>
    public bool isRunActive = false;

    /// <summary>현재 Zone 번호 (1~3)</summary>
    public int currentZone = 1;

    /// <summary>현재 Floor 번호 (1~5)</summary>
    public int currentFloor = 1;

    /// <summary>런 재현을 위한 시드값</summary>
    public long runSeed = 0;

    /// <summary>현재 파티 크리처 인스턴스 ID 목록 (최대 4)</summary>
    public List<string> partyCreatureIds = new List<string>();

    /// <summary>현재 보관함 크리처 인스턴스 ID 목록 (최대 8)</summary>
    public List<string> storageCreatureIds = new List<string>();

    /// <summary>보유 소울 젬 수</summary>
    public int soulGemCount = 3;

    /// <summary>보유 아이템 ID 목록</summary>
    public List<string> itemIds = new List<string>();
}
