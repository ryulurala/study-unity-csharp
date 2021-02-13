---
title: "Resource Managing"
category: Unity-Framework
tags: [unity, prefab, resource-manager, gameObject]
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

### Resource Manager

#### Prefab -> GameObject 예제

- `Project`에 있는 Prefab을 PrefabTest Script의 `Inspector` 이용

  - ![Preview]()

  ```cs
  public class PrefabTest : MonoBehaviour
  {
      [SerializeField] GameObject prefab; // prefab을 담음
      GameObject tank;

      void Start()
      {
          tank = Instantiate(prefab);   // Prefab -> GameObject

          Destroy(tank, 3.0f);  // 3초 후에 삭제
      }
  }
  ```

#### Resource Manger [X]

- Test

  - ![Preview]()

  ```cs
  public class PrefabTest : MonoBehaviour
  {
      GameObject prefab;  // Prefab을 담을 GameObject
      GameObject tank;

      void Start()
      {
          // "Asset/Resource"가 "/"(root) 에서 불러오기
          prefab = Resources.Load<GameObject>("Prefabs/Tank");
          tank = Instantiate(prefab);

          Destroy(tank, 3.0f);  // 3초 후에 제거
      }
  }
  ```

#### Resource Manger [O]

1. Resource Manager 추가

   ```cs
   public class ResourceManager    // MonoBehaviour 상속 X
   {
       public T Load<T>(string path) where T : Object
       {
           return Resources.Load<T>(path);
       }

       public GameObject Instantiate(string path, Transform parent = null)
       {
           GameObject prefab = Load<GameObject>($"Prefabs/{path}");
           if (prefab == null)
           {
               Debug.Log($"Failed to load prefab: {prefab}");
               return null;
           }
           return Object.Instantiate(prefab, parent);  // Object의 Instantiate
       }

       public void Destroy(GameObject gameObject)
       {
           if (gameObject == null) return;

           Object.Destroy(gameObject);
       }
   }
   ```

2. GameManager에 ResourceManager 추가

   ```cs
   ResourceManager _resorce = new ResourceManager();

   public static ResourceManager Resource { get { return Instance._resorce; } }
   ```

3. Test

   - ![Preview]()

   ```cs
   public class PrefabTest : MonoBehaviour
   {
       GameObject tank;

       void Start()
       {
           tank = GameManager.Resource.Instantiate("Tank");

           Destroy(tank, 3.0f);
       }
   }
   ```

---
