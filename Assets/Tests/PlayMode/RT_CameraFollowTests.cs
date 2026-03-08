using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// RT_CameraFollow 단위 테스트 (PlayMode)
/// </summary>
[TestFixture]
public class RT_CameraFollowTests
{
    private GameObject    _camObj;
    private GameObject    _targetObj;
    private RT_CameraFollow _follow;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _camObj = new GameObject("Camera_Test");
        _camObj.AddComponent<Camera>();
        _follow = _camObj.AddComponent<RT_CameraFollow>();

        _targetObj = new GameObject("Target_Test");

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (_camObj    != null) Object.Destroy(_camObj);
        if (_targetObj != null) Object.Destroy(_targetObj);
        yield return null;
    }

    // ─────────────────────────────────────────
    // 타겟 없음
    // ─────────────────────────────────────────

    [UnityTest]
    public IEnumerator Target_없을때_카메라_위치가_변하지않는다()
    {
        _camObj.transform.position = new Vector3(5f, 3f, -10f);
        Vector3 before = _camObj.transform.position;

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(before, _camObj.transform.position,
            "타겟이 없을 때 카메라 위치가 변하면 안 됩니다.");
    }

    // ─────────────────────────────────────────
    // 타겟 추적
    // ─────────────────────────────────────────

    [UnityTest]
    public IEnumerator Target_설정시_XY가_타겟_방향으로_이동한다()
    {
        // 카메라를 원점, 타겟을 (10, 10, 0)에 배치
        _camObj.transform.position    = new Vector3(0f, 0f, -10f);
        _targetObj.transform.position = new Vector3(10f, 10f, 0f);

        // 리플렉션으로 _target 설정
        typeof(RT_CameraFollow)
            .GetField("_target", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(_follow, _targetObj.transform);

        yield return new WaitForSeconds(0.5f);

        Vector3 pos = _camObj.transform.position;
        Assert.Greater(pos.x, 0f, "카메라 X가 타겟 방향으로 이동해야 합니다.");
        Assert.Greater(pos.y, 0f, "카메라 Y가 타겟 방향으로 이동해야 합니다.");
    }

    [UnityTest]
    public IEnumerator Target_추적시_Z_좌표가_변하지않는다()
    {
        float originalZ = -10f;
        _camObj.transform.position    = new Vector3(0f, 0f, originalZ);
        _targetObj.transform.position = new Vector3(5f, 5f, 0f);

        typeof(RT_CameraFollow)
            .GetField("_target", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(_follow, _targetObj.transform);

        yield return new WaitForSeconds(0.3f);

        Assert.AreEqual(originalZ, _camObj.transform.position.z,
            "카메라 추적 중 Z 좌표가 변하면 안 됩니다.");
    }

    [UnityTest]
    public IEnumerator Target_동일위치면_즉시_도달하지않는다_Lerp확인()
    {
        // Lerp 특성: 즉시 도달하지 않고 부드럽게 접근
        _camObj.transform.position    = new Vector3(0f, 0f, -10f);
        _targetObj.transform.position = new Vector3(100f, 0f, 0f);

        typeof(RT_CameraFollow)
            .GetField("_target", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(_follow, _targetObj.transform);

        // 단 1프레임 후 — 완전히 도달하지 않아야 함
        yield return null;

        float x = _camObj.transform.position.x;
        Assert.Less(x, 100f,
            "Lerp 방식이므로 1프레임 만에 타겟에 완전히 도달하면 안 됩니다.");
        Assert.Greater(x, 0f,
            "타겟 방향으로 일부 이동은 해야 합니다.");
    }

    [UnityTest]
    public IEnumerator Target_이동하면_카메라가_따라간다()
    {
        _camObj.transform.position    = new Vector3(0f, 0f, -10f);
        _targetObj.transform.position = new Vector3(0f, 0f, 0f);

        typeof(RT_CameraFollow)
            .GetField("_target", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(_follow, _targetObj.transform);

        yield return new WaitForSeconds(0.3f);
        Vector3 posBeforeMove = _camObj.transform.position;

        // 타겟을 멀리 이동
        _targetObj.transform.position = new Vector3(20f, 0f, 0f);
        yield return new WaitForSeconds(0.3f);

        Assert.Greater(_camObj.transform.position.x, posBeforeMove.x,
            "타겟이 이동하면 카메라도 따라가야 합니다.");
    }
}
