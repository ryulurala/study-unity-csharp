using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    void Update()
    {
        // 예제: Q눌렀을 때 Scene 이동
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Build setting 필요
            Manager.Scene.LoadScene(Define.Scene.Login); // sync
        }
    }

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;
        gameObject.GetOrAddComponent<CursorController>();

        // Manager.UI.ShowPopupUI<UI_Button>();
        // Manager.UI.ShowSceneUI<UI_Inven>();

    }
    public override void Clear()
    {

    }
}
