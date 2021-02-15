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
        if (_mode == Define.CameraMode.QuarterView)
        {

            RaycastHit hit;
            Debug.DrawRay(_player.transform.position, _delta, Color.blue);
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
            {
                // 플레이어 ---Ray---> 방해물 --- 카메라일 때,
                // 플레이어와 가로막힌 것에 대해 거리 차의 0.8f 만큼 카메라를 위치시킨다.
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist;

            }
            else
            {
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform);    // 플레이어 좌표를 주시하도록 함.
            }
        }
    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }
}
