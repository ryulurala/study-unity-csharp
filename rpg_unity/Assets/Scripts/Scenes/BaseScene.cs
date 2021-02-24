using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    // 모든 곳에서 get, 상속만 set, 
    public Define.Scene SceneType { get; protected set; } = Define.Scene.UnKnown;

    // Scene에 관련되므로 최초로 실행
    void Awake()
    {
        init();
    }

    // 추가할 코드가 있으므로
    protected virtual void init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            GameManager.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    // Scene이 종료됐을 경우 실행
    public abstract void Clear();
}
