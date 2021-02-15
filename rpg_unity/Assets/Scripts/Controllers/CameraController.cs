using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Define.CameraMode _mode = Define.CameraMode.QuarterView;
    [SerializeField] Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f);  // 플레이어와의 거리 차이 벡터
    [SerializeField] GameObject _player = null;
    void Start()
    {

    }

    void LateUpdate()
    {
        // Update에서 하면 덜덜 거린다.
        // 카메라가 먼저인지 플레이어가 먼지인지 모른다.
        // 따라서, LateUpdate 해야 함.
        if (_mode == Define.CameraMode.QuarterView)
        {
            transform.position = _player.transform.position + _delta;
            transform.LookAt(_player.transform);    // 플레이어 좌표를 주시하도록 함.
        }
    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }
}
