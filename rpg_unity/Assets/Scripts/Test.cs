using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {
        example_2();
    }
    void example_1()
    {
        // LMB 클릭 시(1번)
        if (Input.GetMouseButtonDown(0))
        {
            // 실제 Screen에 Pointing한 마우스 위치(픽셀 좌표)
            Vector3 inputMouse = Input.mousePosition;

            // World에 Pointing한 마우스 위치
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(inputMouse.x, inputMouse.y, Camera.main.nearClipPlane));

            // 현재 좌표 - 이전 좌표 = 방향
            // (World에 Pointing된 마우스 위치 - Camera 현재 좌표) = 카메라 방향
            Vector3 dir = mousePos - Camera.main.transform.position;

            // 단위벡터로 변환(크기가 1)
            dir = dir.normalized;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 100f))
            {
                // Raycast 충돌 발생하면 충돌체 이름 출력
                Debug.Log($"Racast Camera: @{hit.collider.gameObject.name}");
            }

            // 메인 카메라 위치에서 dir 방향으로 100의 길이만큼 1초 동안 빨간색 광선 발사
            Debug.DrawRay(Camera.main.transform.position, dir * 100f, Color.red, 1f);
        }
    }

    void example_2()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // direction을 포함하는 광선
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 0x00000010, Layer 8, 9만 masking
            // int mask = (1 << 8) | (1 << 9);

            LayerMask mask = LayerMask.GetMask("Layer8") | LayerMask.GetMask("Layer9");

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, mask))
            {
                // Layer8이 충돌 발생하면 충돌체 이름 출력
                Debug.Log($"Racast Camera: @{hit.collider.gameObject.name}");
            }

            // 메인 카메라 위치에서 dir 방향으로 100의 길이만큼 1초 동안 빨간색 광선 발사
            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);
        }
    }
}

