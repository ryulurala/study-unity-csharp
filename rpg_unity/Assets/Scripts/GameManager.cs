using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager s_instance;    // 유일성 보장
    static GameManager Instance { get { if (s_instance == null) init(); return s_instance; } }    // get, Property 이용

    InputManager _input = new InputManager();
    ResourceManager _resorce = new ResourceManager();
    public static InputManager Input { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resorce; } }

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
    }
}