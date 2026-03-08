using NUnit.Framework;

/// <summary>
/// GameEvents 이벤트 발행/구독/클리어 단위 테스트 (EditMode)
/// </summary>
[TestFixture]
public class RT_GameEventsTests
{
    [TearDown]
    public void TearDown()
    {
        GameEvents.ClearAllListeners();
    }

    [Test]
    public void RaiseRunStarted_리스너가_호출된다()
    {
        bool called = false;
        GameEvents.OnRunStarted += () => called = true;

        GameEvents.RaiseRunStarted();

        Assert.IsTrue(called, "OnRunStarted 이벤트 리스너가 호출되지 않았습니다.");
    }

    [Test]
    public void RaiseRunEnded_True_리스너가_true로_호출된다()
    {
        bool? result = null;
        GameEvents.OnRunEnded += isCleared => result = isCleared;

        GameEvents.RaiseRunEnded(true);

        Assert.IsTrue(result.HasValue, "OnRunEnded 이벤트 리스너가 호출되지 않았습니다.");
        Assert.IsTrue(result.Value, "클리어 여부가 true여야 합니다.");
    }

    [Test]
    public void RaiseRunEnded_False_리스너가_false로_호출된다()
    {
        bool? result = null;
        GameEvents.OnRunEnded += isCleared => result = isCleared;

        GameEvents.RaiseRunEnded(false);

        Assert.IsTrue(result.HasValue, "OnRunEnded 이벤트 리스너가 호출되지 않았습니다.");
        Assert.IsFalse(result.Value, "클리어 여부가 false여야 합니다.");
    }

    [Test]
    public void ClearAllListeners_이후_이벤트가_무시된다()
    {
        bool called = false;
        GameEvents.OnRunStarted += () => called = true;

        GameEvents.ClearAllListeners();
        GameEvents.RaiseRunStarted();

        Assert.IsFalse(called, "ClearAllListeners 이후에도 리스너가 호출되었습니다.");
    }

    [Test]
    public void RaiseCaptureAttempted_성공_creatureId와_결과가_전달된다()
    {
        string receivedId = null;
        bool? success = null;
        GameEvents.OnCaptureAttempted += (id, s) => { receivedId = id; success = s; };

        GameEvents.RaiseCaptureAttempted("goblin_01", true);

        Assert.AreEqual("goblin_01", receivedId, "creatureId가 올바르게 전달되지 않았습니다.");
        Assert.IsTrue(success.Value, "포획 성공 여부가 true여야 합니다.");
    }

    [Test]
    public void RaiseFloorChanged_Zone과_Floor번호가_전달된다()
    {
        int receivedZone = -1, receivedFloor = -1;
        GameEvents.OnFloorChanged += (z, f) => { receivedZone = z; receivedFloor = f; };

        GameEvents.RaiseFloorChanged(2, 3);

        Assert.AreEqual(2, receivedZone, "Zone 번호가 올바르게 전달되지 않았습니다.");
        Assert.AreEqual(3, receivedFloor, "Floor 번호가 올바르게 전달되지 않았습니다.");
    }

    [Test]
    public void RaiseSoulGemCountChanged_수량이_전달된다()
    {
        int received = -1;
        GameEvents.OnSoulGemCountChanged += count => received = count;

        GameEvents.RaiseSoulGemCountChanged(5);

        Assert.AreEqual(5, received, "소울 젬 수량이 올바르게 전달되지 않았습니다.");
    }

    [Test]
    public void 복수_리스너_모두_호출된다()
    {
        int callCount = 0;
        GameEvents.OnRunStarted += () => callCount++;
        GameEvents.OnRunStarted += () => callCount++;
        GameEvents.OnRunStarted += () => callCount++;

        GameEvents.RaiseRunStarted();

        Assert.AreEqual(3, callCount, "등록된 리스너 3개가 모두 호출되어야 합니다.");
    }
}
