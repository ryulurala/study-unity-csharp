using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    List<GameObject> list = new List<GameObject>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Build setting 필요
            Manager.Scene.LoadScene(Define.Scene.Game); // sync
        }
    }
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;

        for (int i = 0; i < 10; i++)
            list.Add(Manager.Resource.Instantiate("unitychan"));

        StartCoroutine("coru");
    }
    public override void Clear()
    {
        Debug.Log("LoginScene Clear!");
    }

    IEnumerator coru()
    {
        foreach (GameObject obj in list)
        {
            yield return new WaitForSeconds(1.0f);
            Manager.Resource.Destroy(obj);
        }
    }
}
