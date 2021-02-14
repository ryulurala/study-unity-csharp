---
title: "Collision"
category: Unity-Framework
tags: [unity, collision, collider, rigidbody, is-kinematic, trigger]
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

---
