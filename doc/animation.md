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
  bool _moveToDest;
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

### Blending Animations

1. Animator Blend Tree 생성
2. Add Motion
3. Select Motion
4. Parameter, Threshold 설정
   > Automate Thresholds[X]로 하면 수동 설정 가능

|                        1                         |                        2                         |                        3                         |                        4                         |
| :----------------------------------------------: | :----------------------------------------------: | :----------------------------------------------: | :----------------------------------------------: |
| ![blending-1](/uploads/animation/blending-1.png) | ![blending-2](/uploads/animation/blending-2.png) | ![blending-3](/uploads/animation/blending-3.png) | ![blending-4](/uploads/animation/blending-4.png) |

- Blending Animation 실행

  > Mathf.Lerp()를 이용한 Parameter 증감을 부드럽게 나타냄

  ![blending-animation](/uploads/animation/blending-animation.gif)

  ```cs
  bool _moveToDest;
  float _wait_run_ratio;

  if (_moveToDest)
  {
      // 움직일 때
      // _wait_run_ratio부터 1까지 (10.0f * Time.deltaTime)의 비율로 증가
      _wait_run_ratio = Mathf.Lerp(_wait_run_ratio, 1, 10.0f * Time.deltaTime);

      // get animator
      Animator animator = GetComponent<Animator>();

      // Parameter 설정: Parameter에 따라 두 Animation 비율이 전환됨
      animator.SetFloat("wait_run_ratio", _wait_run_ratio);

      // Blending Animation 실행
      animator.Play("WAIT_RUN");
  }
  else
  {
      // 가만히 있을 때
      // _wait_run_ratio부터 0까지 (10.0f * Time.deltaTime)의 비율로 감소
      _wait_run_ratio = Mathf.Lerp(_wait_run_ratio, 0, 10.0f * Time.deltaTime);

      // get animator
      Animator animator = GetComponent<Animator>();

      // Parameter 설정: Parameter에 따라 두 Animation 비율이 전환됨
      animator.SetFloat("wait_run_ratio", _wait_run_ratio);

      // Blending Animation 실행
      animator.Play("WAIT_RUN");
  }
  ```

---
