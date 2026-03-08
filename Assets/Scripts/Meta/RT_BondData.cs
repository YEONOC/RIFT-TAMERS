using System;

/// <summary>
/// 크리처와 테이머 간 유대 데이터
/// SaveData.bondDataList 에 직렬화되어 영구 보존됨
/// </summary>
[Serializable]
public class BondData
{
    /// <summary>연결된 크리처 고유 ID</summary>
    public string creatureId;

    /// <summary>현재 유대 포인트</summary>
    public int bondPoints;

    /// <summary>현재 유대 레벨 (0~5, 0 = 미형성)</summary>
    public int bondLevel;

    /// <summary>
    /// 유대 포인트를 추가하고 레벨업이 발생하면 true 반환
    /// </summary>
    public bool AddPoints(int points)
    {
        bondPoints += points;
        int newLevel = CalculateLevel();
        if (newLevel > bondLevel)
        {
            bondLevel = newLevel;
            return true;
        }
        return false;
    }

    private int CalculateLevel()
    {
        int level = 0;
        int[] thresholds = GameConstants.BOND_LEVEL_THRESHOLDS;
        for (int i = 1; i < thresholds.Length; i++)
        {
            if (bondPoints >= thresholds[i])
                level = i;
            else
                break;
        }
        return level;
    }

    /// <summary>현재 유대 레벨에 따른 스탯 보너스 배율 반환 (예: 레벨 2 → 1.10)</summary>
    public float GetStatBonusMultiplier()
        => 1f + bondLevel * GameConstants.BOND_STAT_BONUS_PER_LEVEL;
}
