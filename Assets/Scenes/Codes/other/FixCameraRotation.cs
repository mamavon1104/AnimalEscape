using UnityEngine;

public class FixCameraRotation : MonoBehaviour
{
    [SerializeField, Header("追従させたいターゲット")]
    private Transform target;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        // ゲーム開始時点のカメラとターゲットの距離（オフセット）を取得
        offset = transform.position - target.position;
    }

    /// <summary>
    /// プレイヤーが移動した後にカメラが移動するようにするためにLateUpdateにする。
    /// </summary>
    void LateUpdate()
    {
        // カメラの位置をターゲットの位置にオフセットを足した場所にする。
        Vector3 targetPosition = target.position + offset;
        transform.position = targetPosition;

        // カメラの回転をターゲットと同じにする（回転を打ち消す）
        transform.rotation = target.rotation;
    }
}