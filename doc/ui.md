---
title: "UI"
category: Unity-Framework
tags: [unity, ui, rect-transform, pivot, ahchors]
date: "2021-02-21"
---

## UI

- `UI`(`User Interface`)

- Unity에서 `UI`가 있으려면 `Canvas`가 필요!

  > `UI`의 가장 root는 꼭 `Canvas`.  
  > UI는 원근법을 적용받지 않음.

### Rect Transform

- Rect Transform
  > 모든 `UI`는 `Rect Transform` 컴포넌트를 가진다.
- Pivot
  > Pivot을 기준으로 `Location`, `Scale`, `Rotation`이 변한다.
- Anchors

  > 기기마다 상이한 `해상도`별로 `UI의 크기`를 유동적으로 설정할 수 있다.  
  > `부모` ~ `Anchor`까지 비율 연산  
  > `Anchor` ~ `자신`까지 고정 연산

|           Anchors 이론            |                 해상도 비율에 따른 UI 크기 조정                 |
| :-------------------------------: | :-------------------------------------------------------------: |
| ![anchor](/uploads/ui/anchor.png) | ![depends-on-resolution](/uploads/ui/depends-on-resoultion.png) |
|   `부모` ~ `Anchor`: 비율 연산    |         비율 연산만 하도록 Anchor를 UI 크기와 일치시킴          |
|    `Anchor` ~ `자신` 고정 연산    |                                                                 |

### Button Event

1. Button Component에서 On Click() 추가
2. Script 작성

   > pulblic 함수로 만든다.

   ```cs
   [SerializeField] Text _text;
   int _score = 0;
   public void OnButtonClicked()
   {
       _score++;
       _text.text = $"점수: {_score}점";
   }
   ```

3. Button Event 실행할 GameObject 연결
   > 함수를 실행할 Script가 연결된 GameObject

|                         1                         |                         3                         |
| :-----------------------------------------------: | :-----------------------------------------------: |
| ![button-event-1](/uploads/ui/button-event-1.png) | ![button-event-3](/uploads/ui/button-event-3.png) |

- 결과

  ![button-event-result](/uploads/ui/button-event-result.gif)

#### Only. UI Click

- InputManager에 추가

  ```cs
  // UI가 Click된 상태인 지
  if (EventSystem.current.IsPointerOverGameObject())
      return;
  ```

---
