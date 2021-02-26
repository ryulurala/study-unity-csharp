using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager    // MonoBehaviour 상속 X
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf("/");
            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject go = GameManager.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        // Original 미리 들고 있기
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab: {original}");
            return null;
        }

        // Pooling 있는지
        if (original.GetComponent<Poolable>() != null)
            return GameManager.Pool.Pop(original, parent).gameObject;

        // Object의 Instantiate
        GameObject go = Object.Instantiate(original, parent);

        // (Clone) 없애기
        go.name = original.name;

        return go;
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject == null)
            return;

        // 만약 Pooling이 필요하면 PoolManager에 부탁
        Poolable poolable = gameObject.GetComponent<Poolable>();
        if (poolable != null)
        {
            GameManager.Pool.Push(poolable);
            return;
        }

        Object.Destroy(gameObject);
    }
}
