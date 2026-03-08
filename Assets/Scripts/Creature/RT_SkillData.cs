using UnityEngine;

/// <summary>
/// 크리처 스킬 정의 데이터 (ScriptableObject)
/// 파일명 형식: Skill_[이름] (예: Skill_FireBreath)
/// Assets/Data/ 하위에 저장
/// </summary>
[CreateAssetMenu(fileName = "Skill_New", menuName = "RiftTamers/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("기본 정보")]
    public string skillId;
    public string displayName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("스킬 속성")]
    public SkillTargetType targetType      = SkillTargetType.SingleEnemy;
    public SkillDamageType damageType      = SkillDamageType.Physical;
    public float           damageMultiplier = 1.0f;
    public float           cooldown         = 5f;

    [Header("상태이상 부여")]
    public bool             appliesStatusEffect;
    public StatusEffectType statusEffectType;
    [Range(0f, 1f)]
    public float            statusEffectChance;
    [Range(1, 3)]
    public int              statusEffectStacks = 1;

    [Header("시너지 태그")]
    public string[] synergyTags;
}
