using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(gameObject);
    }
    public static void BindEvent(this GameObject gameObject, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(gameObject, action, type);
    }
}
