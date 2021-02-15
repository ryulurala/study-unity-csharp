---
title: "Camera"
category: Unity-Framework
tags: [unity, camera, target-texture, camera-controller]
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

#### CameraController

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

---
