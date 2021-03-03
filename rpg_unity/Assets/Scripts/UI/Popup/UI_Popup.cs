using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void init()
    {
        Manager.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopupUI()
    {
        Manager.UI.ClosePopupUI(this);
    }
}
