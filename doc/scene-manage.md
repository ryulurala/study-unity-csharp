---
title: "Scene Managing"
category: Unity-Framework
tags: [unity, scene, scene-manager, event-system]
date: "2021-02-24"
---

## Scene Managing

- Scene Managing

  > Scene과 관련된 모든 부분 관리  
  > Scene Load될 때 초기화될 부분  
  > Scene Exit될 때 실행할 부분: Clear()

- SceneManagerEx
  - BaseScene
    1. LoginScene
    2. LobbyScene
    3. GameScene
    4. ...

### Scene Manager Extended

- 각 Scene의 Load, Clear를 담당한다.

```cs
public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public void LoadScene(Define.Scene type)
    {
        // 이전 Scene Clear()
        CurrentScene.Clear();

        // 다음 Scene Load()
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(Define.Scene type)
    {
        // C# Reflaction
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }
}
```

### BaseScene

- 해당 Scene의 모든 컴포넌트의 시작을 담당
- `Awake()`로 `Start()`보다 호출 시점을 먼저 시작

```cs
public abstract class BaseScene : MonoBehaviour
{
    // 모든 곳에서 get, 상속만 set,
    public Define.Scene SceneType { get; protected set; } = Define.Scene.UnKnown;

    // Scene에 관련되므로 최초로 실행
    void Awake()
    {
        init();
    }

    // 공통된 코드 + 추가할 코드 각 Scene에서 작성
    protected virtual void init()
    {
        // UI의 Event부분을 담당할 EventSystem은 먼저 생성
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Manager.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    // Scene이 종료됐을 경우 실행
    // 모두 다르므로 구현 부분 필수
    public abstract void Clear();
}
```

### (...)Scene Load Example

- `@Scene` GameObject 생성 및 `(...)Scene` Script 연결

  |                `LoginScene`의 `@Scene`                 |                `GameScene`의 `@Scene`                |
  | :----------------------------------------------------: | :--------------------------------------------------: |
  | ![login-scene](/uploads/scene-manager/login-scene.png) | ![game-scene](/uploads/scene-manager/game-scene.png) |

- Build Settings: **필수 !**

  |              `Build Settings`-`Scenes In Build`              |
  | :----------------------------------------------------------: |
  | ![build-settings](/uploads/scene-manager/build-settings.png) |

- `Q` 누를 경우 `Scene` 전환
  ![scene-load-result](/uploads/scene-manager/scene-load.gif)

#### LoginScene

```cs
public class LoginScene : BaseScene
{
    void Update()
    {
        // 예제: Q눌렀을 때 Scene 이동
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Build setting 필요
            // Game Scene 이동
            Manager.Scene.LoadScene(Define.Scene.Game); // sync
        }
    }

    protected override void init()
    {
        // 부모 코드 먼저 실행
        base.init();
        SceneType = Define.Scene.Login;
    }
    public override void Clear()
    {
        Debug.Log("LoginScene Clear!");
    }
}
```

#### GameScene

```cs
public class GameScene : BaseScene
{
    void Update()
    {
        // 예제: Q눌렀을 때 Scene 이동
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Build setting 필요
            // Login Scene 이동
            Manager.Scene.LoadScene(Define.Scene.Login); // sync
        }
    }
    protected override void init()
    {
        // 부모 코드 먼저 실행
        base.init();
        SceneType = Define.Scene.Game;

        // Pop-up UI
        Manager.UI.ShowPopupUI<UI_Button>();

        // Scene UI
        Manager.UI.ShowSceneUI<UI_Inven>();
    }
    public override void Clear()
    {
        Debug.Log("GameScene Clear!");
    }
}
```

---
