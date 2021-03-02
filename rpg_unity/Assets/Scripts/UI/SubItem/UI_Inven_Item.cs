using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base
{
    string _name;
    enum GameObjects
    {
        ItemIcon,
        ItemText,
    }

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects));

        Get<GameObject>((int)GameObjects.ItemText).GetComponent<Text>().text = _name;

        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((PointerEventData) => { Debug.Log($"Item Click: {_name}"); });
    }

    public void SetInfo(string name)
    {
        _name = name;
    }
}
