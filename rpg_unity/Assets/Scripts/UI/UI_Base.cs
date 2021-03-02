using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void init();
    void Start()
    {
        init();
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object   // Reflection 이용
    {
        string[] names = Enum.GetNames(type);   // 이름 변환

        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind: {names[i]}");
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;   // casting
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    public static void BindEvent(GameObject gameObject, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {

        UI_EventHandler eventHandler = Util.GetOrAddComponent<UI_EventHandler>(gameObject);

        switch (type)
        {
            case Define.UIEvent.Click:
                eventHandler.OnClickHandler -= action;
                eventHandler.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                eventHandler.OnDragHandler -= action;
                eventHandler.OnDragHandler += action;
                break;
        }
    }
}
