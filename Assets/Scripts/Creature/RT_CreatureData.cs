using UnityEngine;

/// <summary>
/// 크리처 종류 정의 데이터 (ScriptableObject)
/// 파일명 형식: Creature_[이름] (예: Creature_GoblinWarrior)
/// Assets/Data/ 하위에 저장
/// </summary>
[CreateAssetMenu(fileName = "Creature_New", menuName = "RiftTamers/CreatureData")]
public class CreatureData : ScriptableObject
{
    [Header("기본 정보")]
    public string          creatureId;
    public string          displayName;
    public CreatureGrade   grade;
    public CreatureFaction faction;
    [TextArea] public string loreDescription;
    public Sprite[]        idleSprites;

    [Header("기본 스탯")]
    public float baseHP    = 100f;
    public float baseATK   = 20f;
    public float baseDEF   = 10f;
    public float baseSpeed = 3f;

    [Header("포획")]
    [Range(0f, 1f)]
    public float captureBaseRate        = 0.3f;
    public bool  requiresSpecialCapture;
    [TextArea] public string specialCaptureCondition;

    [Header("스킬 (최대 3개)")]
    public SkillData[] skills;

    [Header("시너지 태그")]
    public string[] synergyTags;

    [Header("도감")]
    public Sprite   codexSprite;
    [TextArea] public string codexDescription;
}
