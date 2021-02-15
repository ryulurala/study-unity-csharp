---
title: "Unity Framework"
category: Unity-Framework
tags: [unity, visual studio code, component, singleton]
date: "2021-02-11"
---

## Unity 기초

### 환경 설정(`Unity`+`VSCode`)

- 그래픽 환경

1. Unity Hub 다운로드 & 설치
2. Unity 설치: `LTS`(Long Term Support) 버전(현재는 `2019.4.20f1`)
3. Unity Project 생성: Template-`3D`

- 스크립트 환경

1. Visual Studio Code 설치
2. VSCode Extension 설치
   - |                     C#                      |                           Debugger for Unity                            |                        Unity Tools                        |                            Unity Code Snippets                            |
     | :-----------------------------------------: | :---------------------------------------------------------------------: | :-------------------------------------------------------: | :-----------------------------------------------------------------------: |
     | ![C#](../uploads/unity-outline/csharp.jpeg) | ![Debugger for Unity](../uploads/unity-outline/debugger-for-unity.jpeg) | ![Unity Tools](../uploads/unity-outline/unity-tools.jpeg) | ![Unity Code Snippets](../uploads/unity-outline/unity-code-snippets.jpeg) |
3. Unity Code Editor 설정
   - | `Preferences`-`External Tools`-`External Script Editor`-`Visual Studio Code` |
     | :--------------------------------------------------------------------------: |
     | ![VSCode Unity Settings](../uploads/unity-outline/VSCode_unity-settings.png) |

### 화면 창(Window)

|   view    |               description                |
| :-------: | :--------------------------------------: |
|   Scene   | (=영화 세트장), GameObject가 배치된 View |
| Hierarchy |       GameObject의 List(Tree 형태)       |
|  Project  |     Project와 관련된 모든 파일 표시      |
|   Game    |   Main Camera에 의해 렌더링 되는 View    |
| Inspector |  선택한 GameObject에 대한 컴포넌트 정보  |
|    ...    |                   ...                    |

### 유용한 단축키

|                       단축키                       |                 description                 |
| :------------------------------------------------: | :-----------------------------------------: |
|                     `Ctrl`+`P`                     |                 Play / Stop                 |
|                 `Ctrl`+`Shift`+`N`                 |               New GameObject                |
|           `RMB Click`+`W`, `A`, `S`, `D`           |              Scene View 움직임              |
|             `RMB Click`+`Mouse Wheel`              |         Scene View 움직임 속도 조절         |
|                     `Ctrl`+`Z`                     |                  되돌리기                   |
|                    `W` in Scene                    |          Move tool(Object Control)          |
|                    `E` in Scene                    |                 Rotate tool                 |
|                    `R` in Scene                    |                 Scale tool                  |
| `Camera GameObject Click` + `Ctrl` + `Shift` + `F` | Scene View와 Camera View의 Transfrom 동기화 |
|               `Ctrl` + `Shift` + `C`               |            Console 창 뜨게 하기             |
|                        ...                         |                     ...                     |

### Component Pattern

- Code를 부품화해서 관리한다.
- Unity에서 모든 GameObject는 컴포넌트 정보에 따라 달라진다.
- C# Script를 작성하여 컴포넌트로 사용한다.

- Unity 에서는 MonoBehaviour를 상속을 해야 컴포넌트로써 실행이 가능

  - 따라서, MonoBehaviour를 상속받는 Class는 `new 클래스명();` 불가능
  - Create C# Script--상속--`MonoBehaviour Class`----`Behaviour`----`Component`----`Object`

- ex) 오브젝트 회전 컴포넌트

  ```cs
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class Test : MonoBehaviour
  {
      void Update()
      {
          transform.Rotate(new Vector3(1.0f, 1.0f, 1.0f));
      }
  }
  ```

  |                              결과                              |
  | :------------------------------------------------------------: |
  | ![Result Rotated](../uploads/unity-outline/result-rotated.gif) |

### Singleton Pattern

- 대부분 관리(`Managing`)하는 객체(`Instance`)는 오직 1개만 존재해야 함.
  - 최초의 생성자로 1번만 객체를 생성.
  - 여러 차례 생성자를 호출하면 최초로 생성된 객체를 `return` 해준다.

#### GameManager 예제

- `GameManager는` 어디서든 접근할 수 있고 객체는 오직 1개만 존재.

  1. `MonoBehaviour` Class를 상속 O

     - `GameObject`로 관리
       > `GameObject.Find("@GameManager");` 이용

     ```cs
     using System.Collections;
     using System.Collections.Generic;
     using UnityEngine;

     public class GameManager : MonoBehaviour // MonoBehaviour를 상속
     {
         private static GameManager instance;    // 유일성 보장
         public static GameManager Instance
         {
             get
             {
                 if (instance == null) init();
                 return instance;
             }
         }    // get, Property 이용

         static void init()
         {
             // 이름으로 GameManager를 찾음.
             GameObject go = GameObject.Find("@GameManager");
             if (go == null)
             {
                 // "@GameManager"의 GameObject가 없다면 Object 생성
                 go = new GameObject { name = "@GameManager" };
                 go.AddComponent<GameManager>();   // Script 컴포넌트 추가
             }
             // 삭제하지 못하게 함, Scene이 이동해도 제거되지 않음.
             DontDestroyOnLoad(go);
             instance = go.GetComponent<GameManager>();
         }
     }
     ```

  2. `MonoBehaviour` Class를 상속 X

     - `Scene` 이동에 신경쓸 필요 없는 장점이 있다.
     - 눈에 보이지 않는 단점으로 보통은 1번 방법 사용

     ```cs
     public class GameManager    // MonoBehaviour를 상속받지 않음.
     {
         private static GameManager instance;    // 유일성 보장
         public static GameManager Instance
         {
             // get, property 이용
             get
             {
                 if (instance == null) instance = new GameManager();
                 return instance;
             }
         }
         public GameManager()
         {
             // 초기화 코드
         }
     }
     ```

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

### Position

- `transform.position`으로 Object를 움직인다.
- 3차원 좌표 `Vector3`를 사용한다.
- `position` vs `localPosition`

  - | transform.postion | transform.localPosition |
    | :---------------: | :---------------------: |
    |   World 좌표계    |      Local 좌표계       |
    |    World 기준     |    해당 Object 기준     |

- World 좌표계 기준으로 움직이기

  - ![Preview](/uploads/transfrom/move-world-base.gif)

  ```cs
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class PlayerController : MonoBehaviour
  {
      // [SerializeField] 를 이용하면 Editor에서 Value 조절 가능
      [SerializeField] float _speed = 10.0f;

      // 1 frame마다 호출된다, 보통 60 Frame = 1 sec
      void Update()
      {
          if (Input.GetKey(KeyCode.W)) // Input: Key 입력과 연관된 Class
              transform.position += Vector3.forward * Time.deltaTime * _speed;
              // Vector3.forword=(0.0f, 0.0f, 1.0f): World 좌표계 기준
          if (Input.GetKey(KeyCode.S))
              transform.position += Vector3.back * Time.deltaTime * _speed;
              // Vector3.back=(0.0f, 0.0f, -1.0f)
          if (Input.GetKey(KeyCode.A))
              transform.position += Vector3.left * Time.deltaTime * _speed;
              // Vector3.left=(-1.0f, 0.0f, 0.0f)
          if (Input.GetKey(KeyCode.D))
              transform.position += Vector3.right * Time.deltaTime * _speed;
              // Vector3.right=(1.0f, 0.0f, 0.0f)
      }
  }
  ```

#### Input Class

- Key 입력에 대한 Class.
- |    `GetKey(KeyCode k)`    |    `GetKeyDown(KeyCode k)`    |  `GetKeyUp(KeyCode k)`   |
  | :-----------------------: | :---------------------------: | :----------------------: |
  | 입력되는 중="ing"(`N 번`) | 입력이 최초로 눌릴 때(`1 번`) | 입력이 끝났을 때(`1 번`) |

#### Time.deltaTime

- 1 Frame당 실행하는 시간.
- (현재 프레임 시간 - 지난 프레임 시간)
- 곱하는 이유

  > 시간과 비례해서 증가  
  > 만약, 100 FPS 컴퓨터와 10 FPS 컴퓨터는 1초당 이동 거리가 다를 것이다.  
  > 따라서 걸린 시간(Time.deltaTime)을 곱해주면 동일한 이동거리의 결과를 보여준다.

#### TransformDirection()

- 오브젝트 기준 좌표계 -> 월드 기준 좌표계
- Local 좌표계-> World 좌표계

  > `InverseTransformDirection()`: World -> Local 좌표

- `TransformDirection()`을 이용한 오브젝트 기준 움직이기

  - ![Preview](/uploads/transfrom/move-object-base1.gif)

  ```cs
  void Update()
  {
      if (Input.GetKey(KeyCode.W))
          transform.position += transform.TransformDirection(Vector3.forward * Time.deltaTime * _speed);
          // 오브젝트 기준 좌표계 -> 월드 기준 좌표계
      if (Input.GetKey(KeyCode.S))
          transform.position += transform.TransformDirection(Vector3.back * Time.deltaTime * _speed);
      if (Input.GetKey(KeyCode.A))
          transform.position += transform.TransformDirection(Vector3.left * Time.deltaTime * _speed);
      if (Input.GetKey(KeyCode.D))
          transform.position += transform.TransformDirection(Vector3.right * Time.deltaTime * _speed);
  }
  ```

#### tramsform.Translate()

- Local 좌표계로 이동해줌.

- 오브젝트 기준으로 움직이기

  - ![Preview](/uploads/transfrom/move-object-base2.gif)

  ```cs
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class PlayerController : MonoBehaviour
  {
      [SerializeField] float _speed = 10.0f;

      void Update()
      {
          if (Input.GetKey(KeyCode.W))
              transform.Translate(Vector3.forward * Time.deltaTime * _speed);
              // 오브젝트 기준으로 Position을 변경한다.
          if (Input.GetKey(KeyCode.S))
              transform.Translate(Vector3.back * Time.deltaTime * _speed);
          if (Input.GetKey(KeyCode.A))
              transform.Translate(Vector3.left * Time.deltaTime * _speed);
          if (Input.GetKey(KeyCode.D))
              transform.Translate(Vector3.right * Time.deltaTime * _speed);
      }
  }

  ```

### Vector3

- `Transform`은 `Vector3`를 사용한다.
- 멤버변수는 `float x`, `float y`, `float z` 로 구성됨.
- `Vector3` 용도

  1. 위치 벡터
     > 좌표로 위치 정의
  2. 방향 벡터

     > (다음 위치 - 현재 위치)  
     > "거리(크기)", "방향"을 알아냄

     - transform.position.magnitude
       > Vector의 크기 반환  
       > 피타고라스 정리 이용(`x*x + y*y + z*z`)
     - transform.position.normalized
       > Vector의 방향 단위 벡터: 크기(`Magnitude`)가 1  
       > 각 `x`, `y`, `z`를 `magnitude`로 나눠준 값

### Rotation

- `transform.rotation(Quaternion)`
  > `eulerAngle`보다 사용 권장
- `transform.eulerAngles(Vector3)`

  > 절댓값으로 넣어줘야 함  
  > **`+=(Increment)` 사용 X**  
  > **공식 문서**: `Increment`를 사용할 경우 360도가 넘어가 오작동 발생

- |           `EulerAngles`           |           `Quaternion`            |
  | :-------------------------------: | :-------------------------------: |
  |              x, y, z              |        x, y, z, w: 사원수         |
  |           Vector3 사용            |      Vector3를 변환해서 사용      |
  |    `Gimbal Lock` 문제 **발생**    |    `Gimbal Lock` 문제 **해결**    |
  | `transform.eulerAngles = Vector3` | `transform.rotation = Quaternion` |

- `Gimbal Lock` 문제
  > 각 축의 연관성으로 회전을 계속하면 축들이 겹쳐 회전이 먹통이 되는 문제  
  > `Quaternion` 으로 해결

#### Y축을 기준으로 회전 예제

- ![Preview](/uploads/transfrom/rotate-y.gif)

1. `transform.eulerAngles`

   > 절대적으로 `Vector3`를 대입한다.  
   > 상태 회전 변수 필요

   ```cs
   float yAngle = 0.0f;
   void Update()
   {
     yAngle += Time.deltaTime * 100.0f;   // 상태 회전 변수 갱신
     transform.eulerAngles = new Vector3(0.0f, yAngle, 0.0f);  // 절대적으로 대입
     // transform.eulerAngles += yAngle;    // 사용 X
   }
   ```

2. `transform.Rotate(Vector3)`

   > `Vector3` 대입  
   > 상대적으로 회전: 현재 회전값 기준

   ```cs
   void Update()
   {
     // 현재 회전 상태 기준으로 회전(상대적)
     transform.Rotate(new Vector3(0.0f, Time.deltaTime * 100.0f, 0.0f));
   }

   ```

3. `Quaternion.Euler(Vector3)`

   ```cs
   float yAngle = 0.0f;
   void Update()
   {
     yAngle += Time.deltaTime * 100.0f;   // 상태 회전 변수 갱신

     // Euler -> Quaternion 변환
     transform.rotation = Quaternion.Euler(new Vector3(0.0f, _yAngle, 0.0f));
   }
   ```

#### 특정 방향 바라보기 예제

- `Quaternion.LookRotation(Vector3)`

  > 원하는 방향을 바라보게 함

  - ![Preview](/uploads/transfrom/look.gif)

  ```cs
  void Update()
  {
      // 월드좌표 기준 바라보게 함
      transform.rotation = Quaternion.LookRotation(Vector3.forward);
  }
  ```

- `Quaternion.Slerp(Vector3, Vector3, float)`

  > 부드럽게 바라보게 함.  
  > Slerp(현재 방향, 목표 방향, 비율)

  - ![Preview](/uploads/transfrom/look-slerp.gif)

  ```cs
  void Update()
  {
      // 현재 방향에서 월드 기준 forward(앞)으로 부드럽게 바라보게 함.
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.3f);
  }
  ```

### Input-Manager

- `Listener` 역할
- 사용자 입력을 체크하고 이벤트로 실행한다.

1. `InputManager` Class 작성

   ```cs
   public class InputManager   // MonoBehaviour를 상속 X
   {
       public Action KeyAction = null;

       public void OnUpdate()  // Listener
       {
           if (!Input.anyKey) return;  // 아무 키가 입력되지 않으면

           if (KeyAction != null)  // 이벤트가 등록돼있는지 확인
               KeyAction.Invoke(); // 모두 알려주고 실행하도록 함.
       }
   }
   ```

2. `GameManager`에서 관리

   ```cs
   public class GameManager : MonoBehaviour
   {
       static GameManager instance;    // 유일성 보장
       static GameManager Instance { get { if (instance == null) init(); return instance; } }    // get, Property 이용

       InputManager input = new InputManager();
       public static InputManager Input { get { return Instance.input; } }

       void Update()
       {
           input.OnUpdate();   // 이벤트를 체크
       }
   }
   ```

3. `PlayerController`에서 등록

   ```cs
   public class PlayerController : MonoBehaviour
   {
       [SerializeField] float _speed = 10.0f;

       void Start()
       {
           // 리스너 등록
           GameManager.Input.KeyAction -= OnKeyboard;  // 두 번 등록 방지
           GameManager.Input.KeyAction += OnKeyboard;
       }

       void OnKeyboard()
       {
           if (Input.GetKey(KeyCode.W))
           {
               transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.3f);
               transform.position += Vector3.forward * Time.deltaTime * _speed;
           }
           if (Input.GetKey(KeyCode.S))
           {
               transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.3f);
               transform.position += Vector3.back * Time.deltaTime * _speed;
           }
           if (Input.GetKey(KeyCode.A))
           {
               transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.3f);
               transform.position += Vector3.left * Time.deltaTime * _speed;
           }
           if (Input.GetKey(KeyCode.D))
           {
               transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.3f);
               transform.position += Vector3.right * Time.deltaTime * _speed;
           }
       }
   }
   ```

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

  - ![Preview](/uploads/resource-manager/prefab-to-gameobject.gif)

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

  - ![Preview](/uploads/resource-manager/prefab-to-gameobject.gif)

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

   - ![Preview](/uploads/resource-manager/prefab-to-gameobject.gif)

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

### 폴더 정리

- `Resources`
  > Scene에서 사용하는 리소스 모음
  - `Prefabs`
    > Game에서 사용할 조립형 GameObject 모음
  - `Arts`
    > 디자이너 분이 주시는 Art 모음(ex. Charater)
    - `Models`
      > 해당 Art에서의 Model 모음  
      > Prefab을 만들기 위한 Asset
    - `Sounds`
      > 해당 Art에서의 Sound 모음
    - `Textures`
      > 해당 Art에서의 Model들의 Texture 모음
    - `Materials`
      > 해당 Art에서의 Model들의 Material 모음  
      > Texture들을 모아 붙인 재질
    - `Animations`
      > 해당 Art에서의 Model들의 Animation 모음
- `Scenes`
  > Unity 에서 사용하는 Scene 모음
- `Scripts`
  > `C#` Scripts

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

## Camera

### Camera Control

#### Camera Component

- Culling Mask
  > 해당 카메라에 보일 Layer를 지정
- Fild of View
  > 화면에 보일 범위 각 지정
- Clipping Planes
  - Near
    > 카메라 객체로부터 가까이 투영되는 면의 거리  
    > Near보다 가까운 거리는 안보임
  - Far
    > 카메라 객체로부터 멀리 투영되는 면의 거리  
    > Far보다 먼 거리는 안보임
- Target Texture
  > 카메라가 렌더링하여 나온 Texture의 결과를 지정할 수 있다.  
  > 미니맵 등으로 쓰임.

#### Camera Controller

- 부모-자식 관계로 카메라 움직이기

  | 위치, 방향 모두 플레이어(= 부모 객체)를 따라간다  |
  | :-----------------------------------------------: |
  | ![parent-child](/uploads/camera/parent-child.gif) |

- 카메라 움직임 code는 `LateUpdate(){ }` 에 작성해야 함.

  > `Unity`는 `Single Thread`이므로  
  > 플레이어와 카메라 움직임 모두 `Update(){ }`에서 하는 경우,  
  > 어떤 움직임이 먼저 실행될 지 모르므로 덜덜 떨리는 모습이 연출된다.

- Preview

  |     Camera 움직임: `Update(){ }`      |        Camera 움직임: `LateUpdate(){ }`         |
  | :-----------------------------------: | :---------------------------------------------: |
  |     Player 움직임: `Update(){ }`      |          `Player 움직임: Update(){ }`           |
  | ![update](/uploads/camera/update.gif) | ![late-update](/uploads/camera/late-update.gif) |

- Mode 설정: `Define.cs`

  ```cs
  public class Define
  {
      public enum CameraMode
      {
          QuarterView,
          xxxView,  // 예시 1
          yyyView,  // 예시 2
          zzzView,  // 예시 3
      }
  }
  ```

- 카메라 움직임: CameraController

  ```cs
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class CameraController : MonoBehaviour
  {
      // Mode 설정
      [SerializeField] Define.CameraMode _mode = Define.CameraMode.QuarterView;
      // 플레이어와의 거리 차이 벡터
      [SerializeField] Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f);
      // Editor에서 플레이어 설정
      [SerializeField] GameObject _player = null;

      void LateUpdate()
      {
          if (_mode == Define.CameraMode.QuarterView)
          {
              // Camera 위치를 플레이어 위치보다 _delta 만큼 떨어져 있도록 고정
              transform.position = _player.transform.position + _delta;

              // 플레이어 좌표를 주시하도록 함.
              transform.LookAt(_player.transform);
          }
      }

      public void SetQuaterView(Vector3 delta)
      {
          _mode = Define.CameraMode.QuarterView;
          _delta = delta;
      }
  }
  ```

#### Moving by Mouse Event

1. `Define.cs`: Mouse Event 종류

   ```cs
   public class Define
   {
       public enum MouseEvent
       {
           Press,  // ex. 디아블로: 누르고 있는 상태로 움직임
           Click,  // ex. LOL: 한 번의 클릭으로만 움직임
       }
   }
   ```

2. `InputManager.cs`: Action 추가

   ```cs
   public class InputManager   // 입력을 체크하고 Event로 전파해줌
   {
       // MouseEvent에 관련된 이벤트만 등록: 파라미터 지정
       public Action<Define.MouseEvent> MouseAction = null;

       public void OnUpdate()  // Listener
       {
           if (MouseAction != null)    // 이벤트가 등록돼있는지 확인
           {
               if (Input.GetMouseButtonDown(0))
               {
                   // LMB 눌렀다 뗄 때, Click에 해당한 리스너에게 알려줌
                   MouseAction.Invoke(Define.MouseEvent.Click);
               }
               else if (Input.GetMouseButton(0))
               {
                   // LMB가 계속 눌려있으면, Press에 해당한 리스너에게 알려줌
                   MouseAction.Invoke(Define.MouseEvent.Press);
               }
           }
       }
   }
   ```

3. `PlayerController.cs`: transform.position 변경

   - |          일반적인 위치 변경           |            `Mathf.Clamp(value, min, max)` 사용            |
     | :-----------------------------------: | :-------------------------------------------------------: |
     |  현 위치 += 단위 방향 X 속도 X 시간   |      현 위치 += 방향 X 제한 거리(= `Mathf.Clamp()`)       |
     |         `transform.position`          |                   `transform.position`                    |
     |                 `+=`                  |                           `+=`                            |
     |           `dir.normalized`            |                     `dir.normalized`                      |
     |                  `*`                  |                            `*`                            |
     |      `_speed * Time.deltaTime;`       | `Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);` |
     | ![normal](/uploads/camera/normal.gif) |        ![mathf-clamp](/uploads/camera/look-at.gif)        |

   - |             `LookAt()` 사용             |                                 `Quaternion.Slerp()` 사용                                  |
     | :-------------------------------------: | :----------------------------------------------------------------------------------------: |
     |      `transform.LookAt(_destPos);`      |                                    `transform.rotation`                                    |
     |                                         |                                            `=`                                             |
     |                                         | `Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);` |
     | ![look-at](/uploads/camera/look-at.gif) |                            ![slerp](/uploads/camera/slerp.gif)                             |

   ```cs
   public class PlayerController : MonoBehaviour
   {
       [SerializeField] float _speed = 10.0f;
       bool _moveToDest = false;
       Vector3 _destPos;

       void Start()
       {
           // 리스너 등록
           GameManager.Input.MouseAction -= OnMouseCliked;   // 두 번 등록 방지
           GameManager.Input.MouseAction += OnMouseCliked;
       }

       void Update()
       {
           if (_moveToDest)
           {
               Vector3 dir = _destPos - transform.position;  // 방향
               if (dir.magnitude < 0.0001f)    // float 오차 범위로
               {
                   // 도착했을 때
                   _moveToDest = false;
               }
               else
               {
                   // 위치
                   float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
                   transform.position += dir.normalized * moveDist;

                   // 회전
                   transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
               }
           }
       }

       void OnMouseCliked(Define.MouseEvent evt)
       {
           if (evt != Define.MouseEvent.Click) return;

           // ScreenToWorldPoint() + direction.normalized
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

           RaycastHit hit;

           // ray를 최대 100f 거리만큼 발사하여 충돌체 정보를 hit에 저장
           if (Physics.Raycast(ray, out hit, 100f))
           {
               // Raycast 충돌 발생하면 목적지로 지정
               _destPos = hit.point;
               Debug.Log($"destPost={_destPos}");
           }

           // 메인 카메라 위치에서 dir 방향으로 100의 길이만큼 1초 동안 빨간색 광선 발사
           Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);
       }
   }
   ```

#### Player와 사이에 벽이 있을 경우

1. `CameraController.cs`: Raycasting

   ```cs
   void LateUpdate()
   {
       if (_mode == Define.CameraMode.QuarterView)
       {

           RaycastHit hit;
           Debug.DrawRay(_player.transform.position, _delta, Color.blue);
           if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
           {
               // 플레이어 ---Ray---> Wall --- 카메라일 때,
               float dist = (hit.point - _player.transform.position).magnitude * 0.8f;

               // 플레이어와 가로막힌 것에 대해 거리 차의 0.8f 만큼 카메라를 위치시킨다.
               transform.position = _player.transform.position + _delta.normalized * dist;

           }
           else
           {
               // 플레이어 ---Ray---> no Wall ---> 카메라일 때,
               transform.position = _player.transform.position + _delta;
               transform.LookAt(_player.transform);
           }
       }
   }
   ```

2. Layer 설정 및 결과

   - |              Layer 설정(`Wall`)               |                             결과                              |
     | :-------------------------------------------: | :-----------------------------------------------------------: |
     | ![layer-wall](/uploads/camera/layer-wall.png) | ![player-wall-camera](/uploads/camera/player-wall-camera.gif) |

---
