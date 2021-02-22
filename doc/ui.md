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

---
