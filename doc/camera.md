---
title: "Camera"
category: Unity-Framework
tags:
  [
    unity,
    camera,
    target-texture,
    camera-controller,
    mouse-event,
    wall,
    layer-mask,
  ]
date: "2021-02-15"
---

## Camera

### Camera Control

#### Camera Component

- Culling Mask
  > 해당 카메라에 보일 Layer를 지정
- Fild of View
  > 화면에 보일 범위 각 지정
- Clipping Planes
  - Near
    > 카메라 객체로부터 가까이 투영되는 면의 거리  
    > Near보다 가까운 거리는 안보임
  - Far
    > 카메라 객체로부터 멀리 투영되는 면의 거리  
    > Far보다 먼 거리는 안보임
- Target Texture
  > 카메라가 렌더링하여 나온 Texture의 결과를 지정할 수 있다.  
  > 미니맵 등으로 쓰임.

#### Camera Controller

- 부모-자식 관계로 카메라 움직이기

  | 위치, 방향 모두 플레이어(= 부모 객체)를 따라간다  |
  | :-----------------------------------------------: |
  | ![parent-child](/uploads/camera/parent-child.gif) |

- 카메라 움직임 code는 `LateUpdate(){ }` 에 작성해야 함.

  > `Unity`는 `Single Thread`이므로  
  > 플레이어와 카메라 움직임 모두 `Update(){ }`에서 하는 경우,  
  > 어떤 움직임이 먼저 실행될 지 모르므로 덜덜 떨리는 모습이 연출된다.

- Preview

  |     Camera 움직임: `Update(){ }`      |        Camera 움직임: `LateUpdate(){ }`         |
  | :-----------------------------------: | :---------------------------------------------: |
  |     Player 움직임: `Update(){ }`      |          `Player 움직임: Update(){ }`           |
  | ![update](/uploads/camera/update.gif) | ![late-update](/uploads/camera/late-update.gif) |

- Mode 설정: `Define.cs`

  ```cs
  public class Define
  {
      public enum CameraMode
      {
          QuarterView,
          xxxView,  // 예시 1
          yyyView,  // 예시 2
          zzzView,  // 예시 3
      }
  }
  ```

- 카메라 움직임: CameraController

  ```cs
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class CameraController : MonoBehaviour
  {
      // Mode 설정
      [SerializeField] Define.CameraMode _mode = Define.CameraMode.QuarterView;
      // 플레이어와의 거리 차이 벡터
      [SerializeField] Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f);
      // Editor에서 플레이어 설정
      [SerializeField] GameObject _player = null;

      void LateUpdate()
      {
          if (_mode == Define.CameraMode.QuarterView)
          {
              // Camera 위치를 플레이어 위치보다 _delta 만큼 떨어져 있도록 고정
              transform.position = _player.transform.position + _delta;

              // 플레이어 좌표를 주시하도록 함.
              transform.LookAt(_player.transform);
          }
      }

      public void SetQuaterView(Vector3 delta)
      {
          _mode = Define.CameraMode.QuarterView;
          _delta = delta;
      }
  }
  ```

#### Moving by Mouse Event

1. `Define.cs`: Mouse Event 종류

   ```cs
   public class Define
   {
       public enum MouseEvent
       {
           Press,  // ex. 디아블로: 누르고 있는 상태로 움직임
           Click,  // ex. LOL: 한 번의 클릭으로만 움직임
       }
   }
   ```

2. `InputManager.cs`: Action 추가

   ```cs
   public class InputManager   // 입력을 체크하고 Event로 전파해줌
   {
       // MouseEvent에 관련된 이벤트만 등록: 파라미터 지정
       public Action<Define.MouseEvent> MouseAction = null;

       public void OnUpdate()  // Listener
       {
           if (MouseAction != null)    // 이벤트가 등록돼있는지 확인
           {
               if (Input.GetMouseButtonDown(0))
               {
                   // LMB 눌렀다 뗄 때, Click에 해당한 리스너에게 알려줌
                   MouseAction.Invoke(Define.MouseEvent.Click);
               }
               else if (Input.GetMouseButton(0))
               {
                   // LMB가 계속 눌려있으면, Press에 해당한 리스너에게 알려줌
                   MouseAction.Invoke(Define.MouseEvent.Press);
               }
           }
       }
   }
   ```

3. `PlayerController.cs`: transform.position 변경

   - |          일반적인 위치 변경           |            `Mathf.Clamp(value, min, max)` 사용            |
     | :-----------------------------------: | :-------------------------------------------------------: |
     |  현 위치 += 단위 방향 X 속도 X 시간   |      현 위치 += 방향 X 제한 거리(= `Mathf.Clamp()`)       |
     |         `transform.position`          |                   `transform.position`                    |
     |                 `+=`                  |                           `+=`                            |
     |           `dir.normalized`            |                     `dir.normalized`                      |
     |                  `*`                  |                            `*`                            |
     |      `_speed * Time.deltaTime;`       | `Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);` |
     | ![normal](/uploads/camera/normal.gif) |        ![mathf-clamp](/uploads/camera/look-at.gif)        |

   - |             `LookAt()` 사용             |                                 `Quaternion.Slerp()` 사용                                  |
     | :-------------------------------------: | :----------------------------------------------------------------------------------------: |
     |      `transform.LookAt(_destPos);`      |                                    `transform.rotation`                                    |
     |                                         |                                            `=`                                             |
     |                                         | `Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);` |
     | ![look-at](/uploads/camera/look-at.gif) |                            ![slerp](/uploads/camera/slerp.gif)                             |

   ```cs
   public class PlayerController : MonoBehaviour
   {
       [SerializeField] float _speed = 10.0f;
       bool _moveToDest = false;
       Vector3 _destPos;

       void Start()
       {
           // 리스너 등록
           GameManager.Input.MouseAction -= OnMouseCliked;   // 두 번 등록 방지
           GameManager.Input.MouseAction += OnMouseCliked;
       }

       void Update()
       {
           if (_moveToDest)
           {
               Vector3 dir = _destPos - transform.position;  // 방향
               if (dir.magnitude < 0.0001f)    // float 오차 범위로
               {
                   // 도착했을 때
                   _moveToDest = false;
               }
               else
               {
                   // 위치
                   float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
                   transform.position += dir.normalized * moveDist;

                   // 회전
                   transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
               }
           }
       }

       void OnMouseCliked(Define.MouseEvent evt)
       {
           if (evt != Define.MouseEvent.Click) return;

           // ScreenToWorldPoint() + direction.normalized
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

           RaycastHit hit;

           // ray를 최대 100f 거리만큼 발사하여 충돌체 정보를 hit에 저장
           if (Physics.Raycast(ray, out hit, 100f))
           {
               // Raycast 충돌 발생하면 목적지로 지정
               _destPos = hit.point;
               Debug.Log($"destPost={_destPos}");
           }

           // 메인 카메라 위치에서 dir 방향으로 100의 길이만큼 1초 동안 빨간색 광선 발사
           Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);
       }
   }
   ```

#### Player와 사이에 벽이 있을 경우

1. `CameraController.cs`: Raycasting

   ```cs
   void LateUpdate()
   {
       if (_mode == Define.CameraMode.QuarterView)
       {

           RaycastHit hit;
           Debug.DrawRay(_player.transform.position, _delta, Color.blue);
           if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
           {
               // 플레이어 ---Ray---> Wall --- 카메라일 때,
               float dist = (hit.point - _player.transform.position).magnitude * 0.8f;

               // 플레이어와 가로막힌 것에 대해 거리 차의 0.8f 만큼 카메라를 위치시킨다.
               transform.position = _player.transform.position + _delta.normalized * dist;

           }
           else
           {
               // 플레이어 ---Ray---> no Wall ---> 카메라일 때,
               transform.position = _player.transform.position + _delta;
               transform.LookAt(_player.transform);
           }
       }
   }
   ```

2. Layer 설정 및 결과

   - |              Layer 설정(`Wall`)               |                             결과                              |
     | :-------------------------------------------: | :-----------------------------------------------------------: |
     | ![layer-wall](/uploads/camera/layer-wall.png) | ![player-wall-camera](/uploads/camera/player-wall-camera.gif) |

---
