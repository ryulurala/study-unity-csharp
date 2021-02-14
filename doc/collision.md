---
title: "Collision"
category: Unity-Framework
tags:
  [
    unity,
    collision,
    collider,
    rigidbody,
    is-kinematic,
    trigger,
    ray-cast,
    projection,
    ray,
    layer-mask,
    tag,
  ]
date: "2021-02-14"
---

## Collision: 충돌

### Collider: 충돌체

- Rigidbody: 강체

  > 공식 API 문서: Object를 물리적인 시뮬레이션으로 위치를 조절한다.

  - Mass: 질량(kg)
  - Use Gravity: 중력
  - Is Kinematic
    > `Physics`가 `Rigidbody`에 영향 여부 결정  
    > 해당 속성을 설정하고 충돌 판정을 하는데 주로 사용한다.
  - Constraint
    - Position: 특정 축(`x`, `y`, `z`)에 대한 위치 고정
    - Rotation: 특정 축(`x`, `y`, `z`)에 대한 회전 고정

- Collider: 충돌체

  > 공식 API 문서: `Rigidbody`와 함께 사용하여 물리적 상호작용을 적용한다.  
  > `Is Kinematic`으로 물리적 상호작용을 안할 수 있다.

### Collision vs Trigger

|        Collision        |         Trigger         |
| :---------------------: | :---------------------: |
|       Collider[O]       |       Collider[O]       |
| 나 or 상대 Rigidbody[O] | 나 or 상대 Rigidbody[O] |
|      Is Trigger[X]      |      Is Trigger[O]      |
|  Is Kinematic[O or X]   |  Is Kinematic[O or X]   |

- 요약

  - `Collision`과 `Trigger`의 차이는 `Is Trigger` 차이!
  - `Collision`과 `Trigger`는 둘 중 하나는 `Rigidbody`를 가지고 있어야 한다.
  - `Trigger`의 비용이 더 적다.

- `Enter` vs `Stay` vs `Exit`

  |             Enter             |            Stay             |             Exit              |
  | :---------------------------: | :-------------------------: | :---------------------------: |
  | 충돌, 트리거가 발생 시작(1번) | 충돌, 트리거가 발생 중(N번) | 충돌, 트리거가 발생 종료(1번) |

- Code

  ```cs
  // Trigger: Is Trigger[O]
  void OnTriggerEnter(Collider other) { } // 발생 시작(1번)
  void OnTriggerStay(Collider other) { }  // 발생 중(N번)
  void OnTriggerExit(Collider other) { }  // 발생 끝(1번)

  // Collision: Is Trigger[X]
  void OnCollisionEnter(Collision other) { }  // 발생 시작(1번)
  void OnCollisionStay(Collision other) { }   // 발생 중(N번)
  void OnCollisionExit(Collision other) { }   // 발생 끝(1번)
  ```

### Raycast

- 광선을 쏴서 충돌 정보를 추출

#### Physics.Raycast(), Physics.RaycastAll(), Debug.DrawRay()

|                               `Physics.Raycast()`                                |                   `Physics.RaycastAll()`                   |                           `Debug.DrawRay()`                            |
| :------------------------------------------------------------------------------: | :--------------------------------------------------------: | :--------------------------------------------------------------------: |
|                                    개발 용도                                     |                         개발 용도                          |                              디버깅 용도                               |
|                           충돌 발생 여부(`bool`) 반환                            |    충돌 발생 정보를 담은 객체 배열(`RaycastHit[]`) 반환    |                                 `void`                                 |
|                       처음 충돌한 객체 정보만 알 수 있다.                        |             충돌한 객체 정보 모두 알 수 있다.              |                        충돌 객체 정보 확인 불가                        |
|                                     가시성 X                                     |                          가시성 X                          |              `Scene`에서 확인 가능, 광선 Color 설정 가능               |
| (`Vector3 start`, `Vector3 dir`, `RaycastHit hitInfo`, `float maxDistance`), ... | (`Vector3 start`, `Vector3 dir`, `float maxDistance`), ... | (`Vector3 start`, `Vector3 dir`, `Color color`, `float duration`), ... |
|                  `maxDistance`를 설정 안하면 충돌할 때까지 발사                  |          `maxDistance`를 설정 안하면 끝까지 발사           |                        방향 벡터 크기만큼 발사                         |

#### Debug.DrawRay 예제

- ![preview](/uploads/collision/debug-drawRay.gif)

```cs
// 현재 position에서 World 기준 (0, 0, 1) 방향으로 Red 색깔로 광선 발사
Debug.DrawRay(transform.position, Vector3.forward, Color.red);
```

#### Physics.Racast()

- `Raycast()` 처음 충돌체 정보만 추출 가능
- ![preview](/uploads/collision/physics-raycast.gif)

```cs
// 플레이어 기준 forward로 변환
Vector3 look = transform.TransformDirection(Vector3.forward);

// hit된 정보를 담고 있는 객체
RaycastHit hit;

// 현재 position+(0, 1, 0)에서 look 방향으로 최대 10의 길이로 광선 발사한 정보를 hit에 저장
if (Physics.Raycast(transform.position + Vector3.up, look, out hit, 10))
{
    // Raycast로 충돌이 발생하면 이름 출력
    Debug.Log($"RayCast: {hit.collider.gameObject.name}");
}

// 현재 position+(0, 1, 0)에서 look 방향으로 10의 길이로 Red 색깔로 광선 발사
Debug.DrawRay(transform.position + Vector3.up, look * 10, Color.red);
```

#### Physics.RaycastAll()

- `RaycastAll()` 충돌체 정보 모두 추출 가능
- ![preview](/uploads/collision/physics-raycastAll.gif)

```cs
// 플레이어 기준 forward로 변환
Vector3 look = transform.TransformDirection(Vector3.forward);

// 충돌 발생한 정보를 담는 객체 배열
RaycastHit[] hits;

// 현재 position+(0, 1, 0)에서 look 방향으로 최대 10의 길이로 광선 발사한 정보 객체 배열을 반환
hits = Physics.RaycastAll(transform.position + Vector3.up, look, 10);

foreach (RaycastHit hit in hits)
{
    // 정보를 담고있는 객체 배열 모두 출력
    Debug.Log($"RayCast: {hit.collider.gameObject.name}");
}

// 현재 position+(0, 1, 0)에서 look 방향으로 10의 길이로 Red 색깔로 광선 발사
Debug.DrawRay(transform.position + Vector3.up, look * 10, Color.red);
```

### 투영: Projection

- 3D ---> 2D
- `World 좌표계(x, y, z)`에서 `Screen 좌표계(x, y)`로 투영(`Prejection`)
- `Local 좌표계` --`Convert`--> `World 좌표계` --`Projection`--> `Screen 좌표계`

|                            투영 과정                             |
| :--------------------------------------------------------------: |
| ![projection process](/uploads/collision/projection-process.gif) |

- ![screen location](/uploads/collision/screen-location.gif)

```cs
// Screen 좌표 픽셀 반환
Vector3 pixel = Input.mousePosition;
Debug.Log($"Screen 좌표의 픽셀: {pixel}");

// Scrrent 비율 좌표(0 ~ 1)
Vector3 ratio = Camera.main.ScreenToViewportPoint(Input.mousePosition);
Debug.Log($"Screen 좌표의 비율: {ratio}");
```

### Screen(Mouse-Position) to World

- ![screen to world](/uploads/collision/screen-to-world.gif)

1. Version 1

   1. 화면 마우스 위치 --> 월드 좌표계
   2. 방향 추출(현재 좌표 - 이전 좌표)
   3. 방향 단위벡터 변환
   4. Raycasting

   ```cs
   // LMB 클릭 시(1번)
   if (Input.GetMouseButtonDown(0))
   {
       // 실제 Screen에 Pointing한 마우스 위치(픽셀 좌표)
       Vector3 inputMouse = Input.mousePosition;

       // World에 Pointing한 마우스 위치
       Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(inputMouse.x, inputMouse.y, Camera.main.nearClipPlane));

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
   ```

2. Version 2

   1. `ScreenPointToRay()` 이용하여 화면에 입력된 마우스 위치 광선 추출
   2. Raycasting

   ```cs
   if (Input.GetMouseButtonDown(0))
   {
       // ScreenToWorldPoint() + direction.normalized
       Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

       RaycastHit hit;

       // Raycast overload func
       // ray를 최대 100f 거리만큼 발사하여 충돌체 정보를 hit에 저장
       if (Physics.Raycast(ray, out hit, 100f))
       {
           // Raycast 충돌 발생하면 충돌체 이름 출력
           Debug.Log($"Racast Camera: @{hit.collider.gameObject.name}");
       }

       // 메인 카메라 위치에서 dir 방향으로 100의 길이만큼 1초 동안 빨간색 광선 발사
       Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);
   }
   ```

### 충돌 판정 최적화

- `Raycasting` 연산은 부하가 많기 때문에 최적화가 필요하다.
- `LayerMask`와 `Tag`(?)를 이용하여 충돌 판정을 조절한다.

|                 Layer                  |                Tag                 |
| :------------------------------------: | :--------------------------------: |
| ![layer](/uploads/collision/layer.png) | ![tag](/uploads/collision/tag.png) |

#### LayerMask

- `Layer`(묶음)별로 충돌 판정을 조절할 수 있다.
- `Layer` 갯수는 `32`개, Why. `Int(32 bits)`를 이용한 `bit-masking`

  ```cs
  // 0x00000010, Layer 8, 9만 masking
  int mask = (1 << 8) | (1 << 9); // OR 연산

  // 위 구문과 동일, string으로 접근
  LayerMask mask = LayerMask.GetMask("Layer8") | LayerMask.GetMask("Layer9");
  ```

- Test

  ```cs
  if (Input.GetMouseButtonDown(0))
  {
      // direction을 포함하는 광선
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      // 0x00000010, Layer 8, 9만 masking
      int mask = (1 << 8) | (1 << 9);

      RaycastHit hit;
      if (Physics.Raycast(ray, out hit, 100f, mask))
      {
          // Layer8, 9의 충돌체와 발생하면 이름 출력
          Debug.Log($"Racast Camera: @{hit.collider.gameObject.name}");
      }

      // 메인 카메라 위치에서 dir 방향으로 100의 길이만큼 1초 동안 빨간색 광선 발사
      Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);
  }
  ```

#### Tag

- `Layer`는 묶음 표현이면, `Tag`는 하나 or 여러 객체를 표현

```cs
gameObject.tag; // 해당 객체의 tag 추출
gameObject.name;  // 해당 객체의 name 추출
```

---
