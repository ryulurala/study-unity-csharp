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
        return Object.Instantiate(prefab, parent);  // Object의 Instantiate
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject == null) return;

        Object.Destroy(gameObject);
    }
}
