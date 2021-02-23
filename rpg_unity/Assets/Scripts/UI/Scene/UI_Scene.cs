using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    public override void init()
    {
        GameManager.UI.SetCanvas(gameObject, false);
    }
}
