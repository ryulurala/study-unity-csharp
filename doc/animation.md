---
title: "animation"
category: Unity-Framework
tags: [unity, animation, animator]
date: "2021-02-19"
---

## Animation

- 동작이나 모양이 연속으로 나타난 동영상 캡쳐 파일

  > `Animation`은 Artist분이 미리 다른 2D / 3D 엔진으로부터 만들어 준다.

- `Animation Re-targeting`

  > Animation type이 `Humanoid`로 동일하다면 해당 모델에 Animation을 입힐 수 있다.

- `Animator` vs `Animation`

  |                                `Animator`                                |          `Animation`           |
  | :----------------------------------------------------------------------: | :----------------------------: |
  | `Animation Clip`을 `Animator`에 등록하여 `Animation Clip` 실행(**권장**) | Legacy한 `Animation Clip` 실행 |

### Animation 실행 절차

1. `Project`-`Animator Controller` 생성
2. `Animator tab`에서 Drag & Drop 으로 `Animation Clip` 등록
3. Player에 `Animator Component` 등록
4. `Project`-`Animator Controller`를 `Animator Component`에 연결

|                       1                        |                       2                        |                        3, 4                        |
| :--------------------------------------------: | :--------------------------------------------: | :------------------------------------------------: |
| ![process-1](/uploads/animation/process-1.png) | ![process-2](/uploads/animation/process-2.png) | ![process-3-4](/uploads/animation/process-3-4.png) |

- Wait, Run 활용 예제

  ![play-animation](/uploads/animation/play-animation.gif)

  ```cs
  if (_moveToDest)
  {
      // 움직일 때
      // get animator
      Animator animator = GetComponent<Animator>();

      // Play "RUN" animation
      animator.Play("RUN");
  }
  else
  {
      // 가만히 있을 때
      // get animator
      Animator animator = GetComponent<Animator>();

      // Play "WAIT" animation
      animator.Play("WAIT");
  }
  ```
