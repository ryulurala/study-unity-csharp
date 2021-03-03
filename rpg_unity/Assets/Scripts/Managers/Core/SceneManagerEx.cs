using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public void LoadScene(Define.Scene type)
    {
        Manager.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(Define.Scene type)
    {
        // Reflaction
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }
    public void Clear()
    {
        CurrentScene.Clear();
    }
}
