using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject gameObject) where T : UnityEngine.Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
            component = gameObject.AddComponent<T>();

        return component;
    }
    public static GameObject FindChild(GameObject gameObject, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(gameObject, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject gameObject, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (gameObject == null)
            return null;

        if (recursive == false)
        {
            // 재귀 X, 직속 자식만
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
            // 재귀 O, 자식의 자식도 찾음
            foreach (T component in gameObject.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
}
