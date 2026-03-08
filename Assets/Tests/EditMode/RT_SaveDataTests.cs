using NUnit.Framework;
using System.Collections.Generic;

/// <summary>
/// SaveData 기본값 및 구조 단위 테스트 (EditMode)
/// </summary>
[TestFixture]
public class RT_SaveDataTests
{
    private SaveData _data;

    [SetUp]
    public void SetUp()
    {
        _data = new SaveData();
    }

    [Test]
    public void 기본_tamerLevel은_1이다()
    {
        Assert.AreEqual(1, _data.tamerLevel);
    }

    [Test]
    public void 기본_tamerExp는_0이다()
    {
        Assert.AreEqual(0, _data.tamerExp);
    }

    [Test]
    public void 기본_isRunActive는_false이다()
    {
        Assert.IsFalse(_data.isRunActive);
    }

    [Test]
    public void 기본_currentZone은_1이다()
    {
        Assert.AreEqual(1, _data.currentZone);
    }

    [Test]
    public void 기본_currentFloor는_1이다()
    {
        Assert.AreEqual(1, _data.currentFloor);
    }

    [Test]
    public void 기본_soulGemCount는_3이다()
    {
        Assert.AreEqual(3, _data.soulGemCount);
    }

    [Test]
    public void 기본_해금_던전에_zone1이_포함된다()
    {
        Assert.IsTrue(_data.unlockedDungeons.Contains("dungeon_zone1"),
            "기본 해금 던전에 dungeon_zone1이 포함되어야 합니다.");
    }

    [Test]
    public void 리스트_필드가_null이_아니다()
    {
        Assert.IsNotNull(_data.bondDataList,       "bondDataList가 null입니다.");
        Assert.IsNotNull(_data.unlockedCreatureIds, "unlockedCreatureIds가 null입니다.");
        Assert.IsNotNull(_data.partyCreatureIds,    "partyCreatureIds가 null입니다.");
        Assert.IsNotNull(_data.storageCreatureIds,  "storageCreatureIds가 null입니다.");
        Assert.IsNotNull(_data.itemIds,             "itemIds가 null입니다.");
    }

    [Test]
    public void 기본_runSeed는_0이다()
    {
        Assert.AreEqual(0L, _data.runSeed);
    }

    [Test]
    public void 기본_tamerSkillPoints는_0이다()
    {
        Assert.AreEqual(0, _data.tamerSkillPoints);
    }
}
