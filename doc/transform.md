---
title: "About Transform"
category: Unity-Framework
tags:
  [
    unity,
    transform,
    gameObject,
    position,
    rotation,
    scale,
    eulerAngls,
    quaternion,
    inputManager,
  ]
date: "2021-02-12"
---

## Transform

### Player

- 모든 Player, 게임 물체는 `GameObject`고 이를 제어해 사용한다.
- `Transform` 컴포넌트로 `GameObject`의 `Position`(위치), `Rotation`(회전), `Scale`(크기)를 제어한다.

- In code
  - |         `GameObject`          |            `Transform`             |
    | :---------------------------: | :--------------------------------: |
    | `transform.gameObject`로 접근 |  `transform.position`로 위치 조절  |
    |                               |  `transform.rotation`로 회전 조절  |
    |                               | `transform.localScale`로 크기 조절 |

### Position

- `transform.position`으로 Object를 움직인다.
- 3차원 좌표 `Vector3`를 사용한다.
- `position` vs `localPosition`

  - | transform.postion | transform.localPosition |
    | :---------------: | :---------------------: |
    |   World 좌표계    |      Local 좌표계       |
    |    World 기준     |    해당 Object 기준     |

- World 좌표계 기준으로 움직이기

  ```cs
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class PlayerController : MonoBehaviour
  {
      // [SerializeField] 를 이용하면 Editor에서 Value 조절 가능
      [SerializeField] float _speed = 10.0f;

      // 1 frame마다 호출된다, 보통 60 Frame = 1 sec
      void Update()
      {
          if (Input.GetKey(KeyCode.W)) // Input: Key 입력과 연관된 Class
              transform.position += Vector3.forward * Time.deltaTime * _speed;
              // Vector3.forword=(0.0f, 0.0f, 1.0f): World 좌표계 기준
          if (Input.GetKey(KeyCode.S))
              transform.position += Vector3.back * Time.deltaTime * _speed;
              // Vector3.back=(0.0f, 0.0f, -1.0f)
          if (Input.GetKey(KeyCode.A))
              transform.position += Vector3.left * Time.deltaTime * _speed;
              // Vector3.left=(-1.0f, 0.0f, 0.0f)
          if (Input.GetKey(KeyCode.D))
              transform.position += Vector3.right * Time.deltaTime * _speed;
              // Vector3.right=(1.0f, 0.0f, 0.0f)
      }
  }
  ```

#### Input Class

- Key 입력에 대한 Class.
- |    `GetKey(KeyCode k)`    |    `GetKeyDown(KeyCode k)`    |  `GetKeyUp(KeyCode k)`   |
  | :-----------------------: | :---------------------------: | :----------------------: |
  | 입력되는 중="ing"(`N 번`) | 입력이 최초로 눌릴 때(`1 번`) | 입력이 끝났을 때(`1 번`) |

#### Time.deltaTime

- 1 Frame당 실행하는 시간.
- (현재 프레임 시간 - 지난 프레임 시간)
- 곱하는 이유

  > 시간과 비례해서 증가  
  > 만약, 100 FPS 컴퓨터와 10 FPS 컴퓨터는 1초당 이동 거리가 다를 것이다.  
  > 따라서 걸린 시간(Time.deltaTime)을 곱해주면 동일한 이동거리의 결과를 보여준다.

#### TransformDirection()

- 오브젝트 기준 좌표계 -> 월드 기준 좌표계
- Local 좌표계-> World 좌표계

  > `InverseTransformDirection()`: World -> Local 좌표

- `TransformDirection()`을 이용한 오브젝트 기준 움직이기

  ```cs
  void Update()
  {
      if (Input.GetKey(KeyCode.W))
          transform.position += transform.TransformDirection(Vector3.forward * Time.deltaTime * _speed);
          // 오브젝트 기준 좌표계 -> 월드 기준 좌표계
      if (Input.GetKey(KeyCode.S))
          transform.position += transform.TransformDirection(Vector3.back * Time.deltaTime * _speed);
      if (Input.GetKey(KeyCode.A))
          transform.position += transform.TransformDirection(Vector3.left * Time.deltaTime * _speed);
      if (Input.GetKey(KeyCode.D))
          transform.position += transform.TransformDirection(Vector3.right * Time.deltaTime * _speed);
  }
  ```

#### tramsform.Translate()

- Local 좌표계로 이동해줌.

- 오브젝트 기준으로 움직이기

  ```cs
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class PlayerController : MonoBehaviour
  {
      [SerializeField] float _speed = 10.0f;

      void Update()
      {
          if (Input.GetKey(KeyCode.W))
              transform.Translate(Vector3.forward * Time.deltaTime * _speed);
              // 오브젝트 기준으로 Position을 변경한다.
          if (Input.GetKey(KeyCode.S))
              transform.Translate(Vector3.back * Time.deltaTime * _speed);
          if (Input.GetKey(KeyCode.A))
              transform.Translate(Vector3.left * Time.deltaTime * _speed);
          if (Input.GetKey(KeyCode.D))
              transform.Translate(Vector3.right * Time.deltaTime * _speed);
      }
  }

  ```

---
