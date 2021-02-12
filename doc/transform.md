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

---
