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

        Camera.main.GetComponent<CameraController>().Player = Manager.Game.Spawn(Define.WorldObject.Player, "UnityChan");
        // Manager.Game.Spawn(Define.WorldObject.Monster, "Skeleton");
        GameObject go = new GameObject { name = "SpawningPool" };
        SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
        pool.SetKeepMonsterCount(5);
    }
    public override void Clear()
    {

    }
}
