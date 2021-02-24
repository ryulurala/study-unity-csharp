using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Build setting 필요
            GameManager.Scene.LoadScene(Define.Scene.Game); // sync
        }
    }
    protected override void init()
    {
        base.init();

        SceneType = Define.Scene.Login;
    }
    public override void Clear()
    {
        Debug.Log("LoginScene Clear!");
    }
}
