---
title: "UI"
category: Unity-Framework
tags:
  [
    unity,
    ui,
    rect-transform,
    pivot,
    ahchors,
    ui-bind,
    reflection,
    generic,
    extension,
    I..Handler,
    ui-manager,
    pop-up-ui,
    scene-ui,
    ui-blocker,
    panel,
    image,
    text,
    inventory,
  ]
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

### UI 자동화

- Button Event 연결

  1. Unity Editor를 이용한 Event 연결

     > 프로젝트 규모가 점점 커질수록 매우 복잡하고 난해함.

     ![ex-unity-engine-component](/uploads/ui/ex-unity-component.png)

  2. Code로 Binding 후 Event 연결
     1. enum 정의
        > UI name과 Scene의 GameObject name을 일치시켜 정의
     2. Binding
        > `UnityEngine.Object[]`로 모두 모음
     3. Get GameObject
        > `enum`값으로 해당 `Component`를 Get.

#### UI Name Binding

1. enum 정의

   > Scene에 해당 `name`을 가진 GameObject가 존재해야 함.

   ```cs
   // Button 모음
   enum Buttons
   {
       // PointButton 이름의 GameObject 존재
       PointButton,
   }
   // Text 모음
   enum Texts
   {
       // PointText 이름의 GameObject 존재
       PointText,
       ScoreText,
   }
   // GameObject 모음
   enum GameObjects
   {
       TestObject,
   }
   // Image 모음
   enum Images
   {
       ItemIcon,
   }
   ```

   - ![same-name](/uploads/ui/same-name.png)

2. Binding

   > C# Reflection 이용  
   > Generic, `<T>` 이용  
   > UnityEngine.Object로 모두 참조할 수 있는 점을 이용.

   - Bind()
     > `Dictionary`에 담기  
     > `<Type, UnityEngine.Object[]>`

   ```cs
   public class UI_Base : MonoBehaviour
   {
       // Component, GameObject의 부모 클래스인 UnityEngine.Object 모음
       // (key, value): (Buttons, Buttons[]), (Texts, Text[])
       Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

       // C# Reflection, Generic 이용
       protected void Bind<T>(Type type) where T : UnityEngine.Object
       {
           // 이름 변환
           string[] names = Enum.GetNames(type);

           UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];

           // Dictionary<>에 Add
           _objects.Add(typeof(T), objects);

           for (int i = 0; i < names.Length; i++)
           {
               if (typeof(T) == typeof(GameObject))
                   objects[i] = Util.FindChild(gameObject, names[i], true);  // GameObject 경우
               else
                   objects[i] = Util.FindChild<T>(gameObject, names[i], true); // 그 외.

               // 해당 이름의 GameObject가 없을 경우
               if (objects[i] == null)
                   Debug.Log($"Failed to bind: {names[i]}");
           }
       }
   }
   ```

   - FindChild()
     > `GameObject type`일 경우  
     > 그 외 type일 경우

   ```cs
   public class Util
   {
       // GameObject일 경우
       public static GameObject FindChild(GameObject gameObject, string name = null, bool recursive = false)
       {
           // Transform을 찾아 GameObject를 리턴
           Transform transform = FindChild<Transform>(gameObject, name, recursive);
           if (transform == null)
               return null;

           return transform.gameObject;
       }

       // GameObject가 아닐 경우
       public static T FindChild<T>(GameObject gameObject, string name = null, bool recursive = false) where T : UnityEngine.Object
       {
           if (gameObject == null)
               return null;

           if (recursive == false)
           {
               // 재귀 X, 해당 GameObject의 바로 아래 자식 컴포넌트만
               for (int i = 0; i < gameObject.transform.childCount; i++)
               {
                   Transform transform = gameObject.transform.GetChild(i);
                   if (string.IsNullOrEmpty(name) || transform.name == name)
                   {
                       T component = transform.GetComponent<T>();
                       if (component != null)
                           return component;
                   }
               }
           }
           else
           {
               // 재귀 O, 해당 GameObject의 모든 자식 컴포넌트
               foreach (T component in gameObject.GetComponentsInChildren<T>())
               {
                   if (string.IsNullOrEmpty(name) || component.name == name)
                       return component;
               }
           }

           return null;
       }
   }
   ```

3. Get GameObject

   > 상속받는 자식들은 사용 가능 하도록 `protected`

   ```cs
   protected T Get<T>(int idx) where T : UnityEngine.Object
   {
       UnityEngine.Object[] objects = null;
       if (_objects.TryGetValue(typeof(T), out objects) == false)
           return null;

       return objects[idx] as T;   // 해당 Type으로 casting
   }

   protected Text GetText(int idx) { return Get<Text>(idx); }
   protected Button GetButton(int idx) { return Get<Button>(idx); }
   protected Image GetImage(int idx) { return Get<Image>(idx); }
   ```

- 예시

  ```cs
  void Start()
  {
      // Binding by reflection
      Bind<Button>(typeof(Buttons));
      Bind<Text>(typeof(Texts));
      Bind<GameObject>(typeof(GameObjects));
      Bind<Image>(typeof(Images));

      // Use
      GetText((int)Texts.ScoreText);  // Text type의 object 리턴
      GetButton((int)Buttons.PointButton) // // Button type의 object 리턴
  }
  ```

#### UI Event Handler

##### Hard coding: Drag 예제

1. EventHandler Interface를 상속 받는 Script 작성

   ```cs
   // 인터페이스 구현
   public class UI_EventHandler : MonoBehaviour, IDragHandler
   {
       public void OnDrag(PointerEventData eventData)
       {
           // Drag 위치로 계속 Object가 따라오도록
           transform.position = eventData.position;
       }
   }
   ```

2. Script Component 사용
   ![connect-script](/uploads/ui/connect-script.png)

- 결과

  ![ui-event-result](/uploads/ui/ui-event-drag.gif)

##### Improve code: Click 예제

1. UI Event 정의

   > `Define.cs`에 정의  
   > ex) Click, Drag, ...

   ```cs
   public enum UIEvent
   {
       Click,
       Drag,
   }
   ```

2. Event Handler Script 작성

   > IPointerClickHandler: UI Click의 인터페이스  
   > IDragHandler: UI Drag의 인터페이스  
   > I...Handler: UI ...의 인터페이스

   - UI_EventHandler

   ```cs
   public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
   {
       // Click Action(Call-back func) 모음
       public Action<PointerEventData> OnClickHandler = null;
       // Drag Action(Call-back func) 모음
       public Action<PointerEventData> OnDragHandler = null;

       public void OnPointerClick(PointerEventData eventData)
       {
           if (OnClickHandler != null)
               OnClickHandler.Invoke(eventData);
       }
       public void OnDrag(PointerEventData eventData)
       {
           if (OnDragHandler != null)
               OnDragHandler.Invoke(eventData);
       }
   }
   ```

3. Event 등록

   > Action 사용  
   > Component Get or Add 사용  
   > `C#`의 Extension 문법 사용

   - AddUIEvent()

   ```cs
   public static void AddUIEvent(GameObject gameObject, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
   {
       // 해당 Script Component가 없을 경우 Add
       // 있을 경우 Get
       UI_EventHandler eventHandler = Util.GetOrAddComponent<UI_EventHandler>(gameObject);

       switch (type)
       {
           case Define.UIEvent.Click:
               eventHandler.OnClickHandler -= action;  // 두 번 등록 방지
               eventHandler.OnClickHandler += action;
               break;
           case Define.UIEvent.Drag:
               eventHandler.OnDragHandler -= action;   // 두 번 등록 방지
               eventHandler.OnDragHandler += action;
               break;
       }
   }
   ```

   - GetOrAddComponent()
     > null이 아닐 경우, Get  
     > null일 경우, Add

   ```cs
   public class Util
   {
       public static T GetOrAddComponent<T>(GameObject gameObject) where T : UnityEngine.Component
       {
           T component = gameObject.GetComponent<T>();
           if (component == null)
               component = gameObject.AddComponent<T>();

           return component;
       }
   }
   ```

   - AddUIEvent() Extension
     > `AddUIEvent(gameObject, action, type)` ---> `gameObject.AddUIEvent(action, type)` 사용 가능하도록

   ```cs
   public static class Extension
   {
       public static void AddUIEvent(this GameObject gameObject, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
       {
           UI_Base.AddUIEvent(gameObject, action, type);
       }
   }
   ```

- 예시

  ```cs
  void Start()
  {
      // Binding
      Bind<Button>(typeof(Buttons));
      Bind<Text>(typeof(Texts));
      Bind<GameObject>(typeof(GameObjects));
      Bind<Image>(typeof(Images));

      // Action(Call-back func) 연결
      // Extension 문법 사용
      GetButton((int)Buttons.PointButton).gameObject.AddUIEvent(OnButtonClicked, Define.UIEvent.Click);
  }

  int _score = 0;
  public void OnButtonClicked(PointerEventData data)
  {
      // Click할 때마다 점수 증가
      _score++;
      GetText((int)Texts.ScoreText).text = $"점수: {_score}점";
  }
  ```

- 결과
  ![ui-event-click](/uploads/ui/ui-event-click.gif)

### UI Manager

- UI 종류 2가지

  - Pop-Up UI

    > 팝업창으로 보여지는 UI  
    > ex) NPC에게 말 걸 때, 인벤토리 창 등.

    ```cs
    public class UI_Popup : UI_Base
    {
        public virtual void init()
        {
            GameManager.UI.SetCanvas(gameObject, true);
        }

        public virtual void ClosePopupUI()
        {
            GameManager.UI.ClosePopupUI(this);
        }
    }
    ```

    - 모든 Pop-up UI는 `UI_Popup` class를 상속받는다.

  - Scene UI

    > 고정적으로 보이는 UI  
    > HP, MP, EXP(경험치) 등

    ```cs
    public class UI_Scene : UI_Base
    {
        public virtual void init()
        {
            GameManager.UI.SetCanvas(gameObject, false);
        }
    }
    ```

    - 모든 Scene UI는 `UI_Scene` class를 상속받는다.

- UI Manager

  > Canvas의 Sort order를 관리  
  > Pop-up UI, Scene UI 관리

  - SetCanvas()
    > Canvas의 order를 설정하는 func
  - ShowPopupUI()
    > Pop-up UI를 생성 및 보여주기
  - ShowSceneUI()
    > Scene UI를 생성 및 보여주기
  - ClosePopupUI()
    > Pop-up UI 닫기
  - CloseAllPopupUI()
    > Pop-up UI 모두 닫기

  ```cs
  public class UIManager
  {
      int _order = 8; // 순서, (0 ~ 7)은 예약
      Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();  // Pop-up UIs
      UI_Scene _SceneUI = null;   // Scene UI

      // root GameObject로 Bind 하는 func --- for. Scene 관리
      public GameObject Root
      {
          get
          {
              GameObject root = GameObject.Find("@UI_Root");
              if (root == null)
                  root = new GameObject { name = "@UI_Root" };
              return root;
          }
      }

      // Canvas order를 설정하는 func
      public void SetCanvas(GameObject go, bool sort = true)
      {
          Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
          canvas.renderMode = RenderMode.ScreenSpaceOverlay;  // Render mode

          // Canvas 안에 Canvas가 중첩할 때, 자식은 자신만의 sorting order를 가짐
          canvas.overrideSorting = true;

          if (sort)
          {
              // pop-up UI 해당
              canvas.sortingOrder = _order;
              _order++;
          }
          else
          {
              // Scene UI 해당
              canvas.sortingOrder = 0;
          }
      }

      // Scene UI를 Show func
      public T ShowSceneUI<T>(string name = null) where T : UI_Scene
      {
          // name이 없을 경우 T(type)의 이름으로
          if (string.IsNullOrEmpty(name))
              name = typeof(T).Name;

          // Prefab 인스턴스화
          GameObject go = GameManager.Resource.Instantiate($"UI/Scene/{name}");
          T sceneUI = Util.GetOrAddComponent<T>(go);
          _SceneUI = sceneUI;

          go.transform.SetParent(Root.transform);

          return sceneUI;
      }

      // Pop-up UI를 Show func
      public T ShowPopupUI<T>(string name = null) where T : UI_Popup
      {
          // name이 없을 경우 T(type)의 이름으로
          if (string.IsNullOrEmpty(name))
              name = typeof(T).Name;

          // Prefab 인스턴스화
          GameObject go = GameManager.Resource.Instantiate($"UI/Popup/{name}");
          T popup = Util.GetOrAddComponent<T>(go);
          _popupStack.Push(popup);

          go.transform.SetParent(Root.transform);

          return popup;
      }

      // Pop-up UI Close func --- safe
      public void ClosePopupUI(UI_Popup popup)
      {
          if (_popupStack.Count == 0)
              return;

          if (_popupStack.Peek() != popup)
          {
              Debug.Log("Close Popup Failed");
              return;
          }

          ClosePopupUI();
      }

      // Pop-up UI Close func
      public void ClosePopupUI()
      {
          if (_popupStack.Count == 0)
              return;

          UI_Popup popup = _popupStack.Pop();
          _order--;

          GameManager.Resource.Destroy(popup.gameObject);
          popup = null; // 더 이상 접근 못하도록
      }

      // Pop-up UI CloseAll func
      public void CloseAllPopupUI()
      {
          while (_popupStack.Count > 0)
              ClosePopupUI();
      }
  }
  ```

#### Pop-up UI Blocker

- Pop-up UI Stack이 쌓였을 경우, 이전의 Pop-up UI는 선택 못하도록 함.
- Canvas의 자식 중 첫 번째를 `Blocker`로 사용
  > Image Component 사용

|                      Before                       |          Image Component 생성           |     위치: 맨 위, Raycast Target [O]     |                      After                      |
| :-----------------------------------------------: | :-------------------------------------: | :-------------------------------------: | :---------------------------------------------: |
| ![blocker-before](/uploads/ui/blocker-before.gif) | ![blocker-1](/uploads/ui/blocker-1.png) | ![blocker-2](/uploads/ui/blocker-2.png) | ![blocker-after](/uploads/ui/blocker-after.gif) |

### Inventory 예제

1. `Canvas` + `Panel` 생성

   - ![create-panel](/uploads/ui/create-panel.png)

2. `Item` 생성
   > Image, Text 모두 설정: `Anchor`, `Pivot`, `Size` 등  
   > 최종적으로 `Prefab`으로 만듦.
   - ![create-item](/uploads/ui/create-item.png)
   -
3. `Layout` 설정: Grid Layout
   > Cell Size, Spacing
   - ![set-layout](/uploads/ui/set-layout.png)
   -
4. Script 작성

   - `UIManager.cs`

     > 종속적인 UI를 만드는 func

   ```cs
   public class UIManager
   {
       // 종속적인 UI Item을 만드는 func
       public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
       {
           // name이 없으면 type의 name으로
           if (string.IsNullOrEmpty(name))
               name = typeof(T).Name;

           // Prefab -> Instance
           GameObject go = GameManager.Resource.Instantiate($"UI/SubItem/{name}");

           // Parent 설정
           if (parent != null)
               go.transform.SetParent(parent.transform);

           return Util.GetOrAddComponent<T>(go);
       }
   }
   ```

   - `UI_Inven.cs`

     > 인벤토리와 관련된 Script  
     > 인벤토리 아이템을 만든다.

   ```cs
   // 인벤토리 관한 Script
   public class UI_Inven : UI_Scene
   {
       enum GameObjects
       {
           GridPanel,
       }
       void Start()
       {
           init();
       }

       public override void init()
       {
           // Set Canvas
           base.init();

           // Binding
           Bind<GameObject>(typeof(GameObjects));

           // Component get
           GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);

           // 처음에 혹시나 있을 아이템 모두 지워줌.
           foreach (Transform child in gridPanel.transform)
               GameManager.Resource.Destroy(child.gameObject);

           // 인벤토리 정보를 참고해서 채워넣음
           for (int i = 0; i < 8; i++)
           {
               // Prefab 인스턴스화
               // 종속적인 아이템 부모랑 연결
               GameObject item = GameManager.UI.MakeSubItem<UI_Inven_Item>(parent: gridPanel.transform).gameObject;

               // UI_Inven_Item Component 연결
               // Extension 문법
               UI_Inven_Item invenIten = item.GetOrAddComponent<UI_Inven_Item>();

               // Instanse화는 Awake()까지 호출
               // Scene Update 시 Start() 호출
               invenIten.SetInfo($"unity {i}");
           }
       }
   }
   ```

   - `UI_Inven_Item.cs`
     > 인벤토리 아이템과 관련된 정보  
     > 정보들 연결

   ```cs
   public class UI_Inven_Item : UI_Base
   {
       string _name; // text

       enum GameObjects
       {
           ItemIcon, // 동일한 name 지정
           ItemText,
       }

       // Prefab -> Instance -> Awake() -> Scene Update() -> Start()
       void Start()
       {
           init();
       }

       public override void init()
       {
           Bind<GameObject>(typeof(GameObjects));

           Get<GameObject>((int)GameObjects.ItemText).GetComponent<Text>().text = _name;

           // Click Event 추가
           // Extension 문법
           Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((PointerEventData) => { Debug.Log($"Item Click: {_name}"); });
       }

       public void SetInfo(string name)
       {
           // text 설정
           _name = name;
       }
   }
   ```

- 결과
  - ![inventory-result](/uploads/ui/inventory-result.gif)

---
