using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager   // 입력을 체크하고 Event로 전파해줌
{
    public Action KeyAction = null;

    public void OnUpdate()  // Listener
    {
        if (!Input.anyKey) return;  // 아무 키가 입력되지 않으면

        if (KeyAction != null)  // 이벤트가 등록돼있는지 확인
            KeyAction.Invoke(); // 사방으로 알려줌
    }
}
