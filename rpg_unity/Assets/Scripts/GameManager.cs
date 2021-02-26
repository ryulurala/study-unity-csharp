using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager s_instance;    // 유일성 보장
    static GameManager Instance { get { if (s_instance == null) init(); return s_instance; } }    // get, Property 이용

    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resorce = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();

    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resorce; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }

    void Update()
    {
        _input.OnUpdate();   // 이벤트를 체크
    }

    static void init()
    {
        GameObject go = GameObject.Find("@GameManager");  // 이름으로 GameManager를 찾음.
        if (go == null)
        {
            // "@GameManager"의 GameObject가 없다면
            go = new GameObject { name = "@GameManager" };   // 게임 오브젝트 생성
            go.AddComponent<GameManager>();     // 스크립트 컴포넌트 추가
        }

        DontDestroyOnLoad(go);  // 삭제하지 못하게 함, Scene이 이동해도 제거되지 않음.
        s_instance = go.GetComponent<GameManager>();

        s_instance._pool.init();
        s_instance._sound.init();
    }

    public static void Clear()
    {
        Input.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }
}