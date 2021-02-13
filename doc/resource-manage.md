---
title: "Resource Managing"
category: Unity-Framework
tags: [unity, prefab]
date: "2021-02-13"
---

## Resource Managing

### Prefab

- `Pre-Fabrication`: 조립식, 미리 만든 물건
- 재사용 `GameObject`
- `Class`-`Instance` 구조에서의 `Class`와 동일

  > 일종의 붕어빵 틀(= `Class`)  
  > 붕어빵: `Instance`(객체)

- Code(C#)

  ```cs
  class Prefab { } // 붕어빵 틀

  class test
  {
      static void Main(string[] args)
      {
          Prefab prefab1 = new Prefab();  // 붕어빵 1
          Prefab prefab2 = new Prefab();  // 붕어빵 2
          Prefab prefab3 = new Prefab();  // 붕어빵 3
      }
  }
  ```

#### Prefab Override

> `Prefab` 속성을 변경

1. `Project`-해당 `Prefab` 클릭 or 더블클릭(상세 설정)
2. `Prefab` 속성 변경

#### Prefab Variants

- `Prefab`을 `Override`한 `Prefab` 생성
  > vs `Original Prefab`은 독립적인 `Prefab` 생성
- Code(C#)

  ```cs
  class Prefab
  {
      float name="original";
  }

  class PrefabVariant: Prefab
  {
      float name="varient";
  }
  ```

#### Nested Prefabs

- `Prefab`을 포함한 `GameObject`를 `Prefab`으로 생성

- Code(C#)

  ```cs
  class NestedPrefab
  {
      Prefab prefab;    // Prefab을 감싸고 있는 Nested Prefab
  }
  class Prefab { }
  ```

---
