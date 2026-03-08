using UnityEngine;

/// <summary>
/// 지정한 대상(플레이어)을 부드럽게 추적하는 카메라 컨트롤러.
/// Main Camera 오브젝트에 부착하고 Target에 플레이어를 지정한다.
/// </summary>
public class RT_CameraFollow : MonoBehaviour
{
    [Header("추적 설정")]
    [SerializeField] private Transform _target;
    [SerializeField] private float     _smoothSpeed = GameConstants.CAMERA_FOLLOW_SMOOTH;

    // ─────────────────────────────────────────
    // 업데이트 (LateUpdate: 플레이어 이동 후 카메라 갱신)
    // ─────────────────────────────────────────

    private void LateUpdate()
    {
        if (_target == null) return;

        Vector3 desired = new Vector3(
            _target.position.x,
            _target.position.y,
            transform.position.z  // Z축(깊이)은 유지
        );

        transform.position = Vector3.Lerp(
            transform.position,
            desired,
            _smoothSpeed * Time.deltaTime
        );
    }

    // ─────────────────────────────────────────
    // 공개 API
    // ─────────────────────────────────────────

    /// <summary>런타임에서 추적 대상을 변경할 때 사용</summary>
    public void SetTarget(Transform target) => _target = target;
}
