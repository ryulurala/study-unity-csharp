---
title: "animation"
category: Unity-Framework
tags: [unity, animation, animator, state-pattern, keyframe, event]
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

### State Pattern

#### `bool state` + `Update() { if-else }`

- 스파게티 코드가 됨.
- 유지보수 불가.

```cs
// bool state
bool idle;
bool isMoving;
bool isSleeping;
bool isJumping;

void Update()
{
    if(isMoving)
    {
        // 움직일 때,
        if(isJumping)
        {
            // 움직이면서 점핑할 때
        }
        else
        {
            // 움직이면서 점핑 안할 때
        }
    }
    else if(isSleeping)
    {
        // 잘 때
    }
    else if(idle)
    {
        // 기본 상태
    }
}
```

#### `Enum State` + `State Func`

- `State`를 `enum`으로 관리.
- `State`마다 `Function` 정의.

```cs
public enum PlayerState
{
    Idle,
    Moving,
    Sleeping,
    Jumping,
}

PlayerState _state = PlayerState.idle;

void UpdateIdle(){ }  // idle 상태 코드
void UpdateMoving(){ }  // moving 상태 코드
void UpdateSleeping(){ }  // sleeping 상태 코드
void UpdateJumping(){ } // jumping 상태 코드

void Update()
{
    // Tick 마다 State가 전환된다.
    // 다음 Tick은 idle, 그 다음 Tick은 Moving, ... 등등.
    switch (_state)
    {
        case PlayerState.Idle:
            UpdateIdle();
            break;
        case PlayerState.Moving:
            UpdateMoving();
            break;
        case PlayerState.Sleeping:
            UpdateSleeping();
            break;
        case PlayerState.Jumping:
            UpdateJumping();
            break;
    }
}
```

### State Machine

- `Parameter` or `Auto`로 `State`를 전환하며 `Animation`을 실행한다.

1. `Animation Clip` 등록
   > = `State`
2. `Make Transition`
3. `Transition` 설정
   > Transition이 있어야 해당 Animation state로 갈 수 있다.  
   > Has Exit Time[O] or [X]: 해당 Animation이 끝나야 Transition될 지.
4. `Condition` 설정
   > `Parameter` 생성  
   > `Parameter` 값에 따라 state가 transition 되도록

|                                1                                 |                                2                                 |                                3                                 |                                4                                 |
| :--------------------------------------------------------------: | :--------------------------------------------------------------: | :--------------------------------------------------------------: | :--------------------------------------------------------------: |
| ![state-transition-1](/uploads/animation/state-transition-1.png) | ![state-transition-2](/uploads/animation/state-transition-2.png) | ![state-transition-3](/uploads/animation/state-transition-3.png) | ![state-transition-4](/uploads/animation/state-transition-4.png) |

- `speed`값으로 애니메이션 제어

  ![state-machine](/uploads/animation/state-machine.gif)

  ```cs
  void UpdateIdle()
  {
      // Get animator
      Animator animator = GetComponent<Animator>();

      // Animator Parameter 제어
      animator.SetFloat("speed", 0);
  }
  void UpdateMoving()
  {
      // Get animator
      Animator animator = GetComponent<Animator>();

      // Animator Parameter 제어
      animator.SetFloat("speed", _speed);
  }
  ```

### KeyFrame Animation

- GameObject에 종속적인 Animation을 Unity Engine에서 제작

  > ex. 카드 뒤집기 in 카드 게임  
  > 카드에 대해서 Animation을 미리 Artist분이 만드는 것이 아닌  
  > 게임만을 위한 카드 Animation을 엔진에서 조작한다.

#### KeyFrame Animation 예제

1. `Animation` Tab
   > `Window`-`Animation`-`Animation`
2. `KeyFrame Animation`을 만들 `GameObject` 선택 후 `Create`
   > Animation + Animator Controller 파일이 생성됨.
3. Add Property
   - `Transform`: 위치, 회전, 크기 조절
   - `Mesh Renderer`: 가시성 조절
   - `Box Collider`: 충돌 여부 조절
4. `Recoding Mode`
   > Turn on recoding mode  
   > `Time`마다 `Property` 변경.  
   > Turn off recoding mode

|                                  1                                   |                                  2                                   |                                  3                                   |                                  4                                   |
| :------------------------------------------------------------------: | :------------------------------------------------------------------: | :------------------------------------------------------------------: | :------------------------------------------------------------------: |
| ![keyFrame-animation-1](/uploads/animation/keyframe-animation-1.png) | ![keyFrame-animation-2](/uploads/animation/keyframe-animation-2.png) | ![keyFrame-animation-3](/uploads/animation/keyframe-animation-3.png) | ![keyFrame-animation-4](/uploads/animation/keyframe-animation-4.png) |

- 결과
  ![keyFrame-animation-result](/uploads/animation/keyframe-animation-result.gif)

### Animation Event

- `Animation`이 실행될 때, 특정 `Frame`에서 `Call-back`방식으로 `Event function`을 실행
- `Sound`, `Effect` 시점을 맞출 수 있다.
  > ex) 발이 땅에 닿았을 시점에서 Sound 발생

#### Animation Event 예제

1. `Add event`
   > 특정 Frame 시점으로 옮김
2. 해당 `GameObject`에 `Script`로 `Call-back Function` 작성

   > public, private 상관 X

   ```cs
   public class AddEventTest : MonoBehaviour
   {
       void AnimationEventFunction()
       {
           Debug.Log("휘리릭~~");
       }
   }
   ```

3. 해당 시점에 `Event Function` 지정

|                               1                                |                               2                                |                               3                                |
| :------------------------------------------------------------: | :------------------------------------------------------------: | :------------------------------------------------------------: |
| ![animation-event-1](/uploads/animation/animation-event-1.png) | ![animation-event-2](/uploads/animation/animation-event-2.png) | ![animation-event-3](/uploads/animation/animation-event-3.png) |

- 결과
  ![animation-event-result](/uploads/animation/animation-event-result.gif)

#### Character Animation Event 예제

1. Model의 Instpector-Animation-Event
2. `Add Event`
   > 특정 Frame 시점으로 옮김
3. `Function` 이름 지정
   > float, int, string, Object 등 Parameter로 받을 수 있다.  
   > 여러 개를 설정해도 하나만 받아진다.
4. 해당 Animation을 사용하는 GameObject에 Script 작성

   > Event Function 이름과 같은 Call-back Function 작성

   ```cs
   public class PlayerController : MonoBehaviour
   {
       void FootSoundEvent(String sound)
       {
           Debug.Log(sound);
       }
   }
   ```

|                                         1                                          |                                         2                                          |                                         3                                          |
| :--------------------------------------------------------------------------------: | :--------------------------------------------------------------------------------: | :--------------------------------------------------------------------------------: |
| ![character-animation-event-1](/uploads/animation/character-animation-event-1.png) | ![character-animation-event-2](/uploads/animation/character-animation-event-2.png) | ![character-animation-event-3](/uploads/animation/character-animation-event-3.png) |

- 결과
  ![character-animation-event-result](/uploads/animation/character-animation-event-result.gif)

---
