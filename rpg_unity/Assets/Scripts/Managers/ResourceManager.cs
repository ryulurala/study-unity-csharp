using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager    // MonoBehaviour 상속 X
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab: {prefab}");
            return null;
        }

        // Object의 Instantiate
        GameObject go = Object.Instantiate(prefab, parent);

        // (Clone) 없애기
        int index = go.name.LastIndexOf("(Clone)");
        if (index > 0)
            go.name = go.name.Substring(0, index);

        return go;
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject == null) return;

        Object.Destroy(gameObject);
    }
}
