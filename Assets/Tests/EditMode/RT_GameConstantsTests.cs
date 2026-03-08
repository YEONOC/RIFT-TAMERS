using NUnit.Framework;

/// <summary>
/// GameConstants 밸런스 수치 유효성 단위 테스트 (EditMode)
/// </summary>
[TestFixture]
public class RT_GameConstantsTests
{
    [Test]
    public void 방_구성_비율_합계가_100이다()
    {
        int total = GameConstants.ROOM_CHANCE_BATTLE
                  + GameConstants.ROOM_CHANCE_EVENT
                  + GameConstants.ROOM_CHANCE_SHOP
                  + GameConstants.ROOM_CHANCE_REST
                  + GameConstants.ROOM_CHANCE_TREASURE
                  + GameConstants.ROOM_CHANCE_CAPTURE_CHALLENGE;

        Assert.AreEqual(100, total, "방 구성 비율의 합이 100이어야 합니다.");
    }

    [Test]
    public void 포획_최소율이_최대율보다_작다()
    {
        Assert.Less(GameConstants.CAPTURE_MIN_RATE, GameConstants.CAPTURE_MAX_RATE,
            "CAPTURE_MIN_RATE가 CAPTURE_MAX_RATE보다 작아야 합니다.");
    }

    [Test]
    public void 포획_최소율이_0보다_크다()
    {
        Assert.Greater(GameConstants.CAPTURE_MIN_RATE, 0f,
            "CAPTURE_MIN_RATE는 0보다 커야 합니다.");
    }

    [Test]
    public void 포획_최대율이_1이하이다()
    {
        Assert.LessOrEqual(GameConstants.CAPTURE_MAX_RATE, 1f,
            "CAPTURE_MAX_RATE는 1 이하여야 합니다.");
    }

    [Test]
    public void 유대_레벨_임계값이_오름차순이다()
    {
        var thresholds = GameConstants.BOND_LEVEL_THRESHOLDS;
        for (int i = 1; i < thresholds.Length; i++)
            Assert.Greater(thresholds[i], thresholds[i - 1],
                $"BOND_LEVEL_THRESHOLDS[{i}]({thresholds[i]})가 [{i-1}]({thresholds[i-1]})보다 커야 합니다.");
    }

    [Test]
    public void 유대_레벨_임계값_개수가_최대레벨_더하기_1이다()
    {
        // 레벨 0~MAX_BOND_LEVEL 까지 (0 포함) 이므로 MAX+1 개
        Assert.AreEqual(GameConstants.MAX_BOND_LEVEL + 1, GameConstants.BOND_LEVEL_THRESHOLDS.Length,
            "BOND_LEVEL_THRESHOLDS 배열 크기가 MAX_BOND_LEVEL + 1이어야 합니다.");
    }

    [Test]
    public void 소울젬_배율이_등급_순서대로_크다()
    {
        Assert.Less(GameConstants.GEM_NORMAL_MULTIPLIER,    GameConstants.GEM_SUPERIOR_MULTIPLIER,
            "일반 젬 배율이 상급보다 작아야 합니다.");
        Assert.Less(GameConstants.GEM_SUPERIOR_MULTIPLIER,  GameConstants.GEM_LEGENDARY_MULTIPLIER,
            "상급 젬 배율이 전설보다 작아야 합니다.");
    }

    [Test]
    public void 크리티컬_배율이_1보다_크다()
    {
        Assert.Greater(GameConstants.CRIT_MULTIPLIER, 1f,
            "크리티컬 배율은 1보다 커야 합니다.");
    }

    [Test]
    public void 속성_유불리_배율이_1을_기준으로_대칭이다()
    {
        float advantage    = GameConstants.TYPE_ADVANTAGE_MULTIPLIER;
        float disadvantage = GameConstants.TYPE_DISADVANTAGE_MULTIPLIER;
        Assert.Greater(advantage, 1f,    "유리 속성 배율은 1보다 커야 합니다.");
        Assert.Less(disadvantage,    1f, "불리 속성 배율은 1보다 작아야 합니다.");
    }

    [Test]
    public void 데미지_최솟값이_1이상이다()
    {
        Assert.GreaterOrEqual(GameConstants.DAMAGE_MIN, 1f,
            "최소 데미지는 1 이상이어야 합니다.");
    }

    [Test]
    public void 테이머_스킬_쿨다운이_양수이다()
    {
        Assert.Greater(GameConstants.SKILL_SOUL_GEM_COOLDOWN,         0f);
        Assert.Greater(GameConstants.SKILL_BATTLE_ROAR_COOLDOWN,      0f);
        Assert.Greater(GameConstants.SKILL_EMERGENCY_RECALL_COOLDOWN, 0f);
    }
}
