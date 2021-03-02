using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager   // 입력을 체크하고 Event로 전파해줌
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;
    float _pressedTime = 0.0f;

    public void OnUpdate()  // Listener
    {
        // if (!Input.anyKey) return;  // 아무 키가 입력되지 않으면

        if (EventSystem.current.IsPointerOverGameObject())  // UI가 Click된 상태인 지
            return;

        if (KeyAction != null)  // 이벤트가 등록돼있는지 확인
            KeyAction.Invoke(); // 사방으로 알려줌

        if (MouseAction != null)    // 이벤트가 등록돼있는지 확인
        {
            if (Input.GetMouseButtonDown(0))
            {
                MouseAction.Invoke(Define.MouseEvent.PointDown);

                // 시간 측정
                _pressedTime = Time.time;
            }
            else if (Input.GetMouseButton(0))
            {
                MouseAction.Invoke(Define.MouseEvent.Press);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (Time.time - _pressedTime < 1.0f)
                    MouseAction.Invoke(Define.MouseEvent.Click);
                else
                    MouseAction.Invoke(Define.MouseEvent.PointUp);
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
