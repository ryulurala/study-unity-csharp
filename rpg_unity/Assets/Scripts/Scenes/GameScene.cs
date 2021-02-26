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
            GameManager.Scene.LoadScene(Define.Scene.Login); // sync
        }
    }

    protected override void init()
    {
        base.init();

        SceneType = Define.Scene.Game;

        // GameManager.UI.ShowPopupUI<UI_Button>();
        GameManager.UI.ShowSceneUI<UI_Inven>();

    }
    public override void Clear()
    {

    }
}
