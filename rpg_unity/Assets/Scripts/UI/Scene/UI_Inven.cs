using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{
    enum GameObjects
    {
        GridPanel,
    }
    public override void init()
    {
        // Set Canvas
        base.init();

        // Binding
        Bind<GameObject>(typeof(GameObjects));

        // Component get
        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);

        // 처음에 혹시나 있을 아이템 모두 지워줌.
        foreach (Transform child in gridPanel.transform)
            Manager.Resource.Destroy(child.gameObject);

        // 인벤토리 정보를 참고해서 채워넣음
        for (int i = 0; i < 8; i++)
        {
            // Prefab 인스턴스화
            GameObject item = Manager.UI.MakeSubItem<UI_Inven_Item>(parent: gridPanel.transform).gameObject;

            // UI_Inven_Item Component 연결
            UI_Inven_Item invenIten = item.GetOrAddComponent<UI_Inven_Item>();

            // Instanse화는 Awake()까지 호출
            // Scene Update 시 Start() 호출
            invenIten.SetInfo($"unity {i}");
        }
    }
}
