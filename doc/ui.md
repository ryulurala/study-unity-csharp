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

|           Anchors 이론            |                해상도 비율에 따른 버튼 크기 조정                |
| :-------------------------------: | :-------------------------------------------------------------: |
| ![anchor](/uploads/ui/anchor.png) | ![depends-on-resolution](/uploads/ui/depends-on-resoultion.png) |
|   `부모` ~ `Anchor`: 비율 연산    |           비율 연산만 하도록 Anchor를 크기와 일치시킴           |
|    `Anchor` ~ `자신` 고정 연산    |                                                                 |

---
