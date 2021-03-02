using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    Texture2D _attackIcon;
    Texture2D _handIcon;
    CursorType _cursorType = CursorType.None;

    enum CursorType
    {
        None,
        Hand,
        Attack,
    }

    void Start()
    {
        _attackIcon = GameManager.Resource.Load<Texture2D>("Textures/Cursors/Attack");
        _handIcon = GameManager.Resource.Load<Texture2D>("Textures/Cursors/Hand");
    }

    void Update()
    {
        UpdateMouseCursor();
    }

    void UpdateMouseCursor()
    {
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Ground") | LayerMask.GetMask("Monster")))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                if (_cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
                    _cursorType = CursorType.Attack;
                }
            }
            else
            {
                if (_cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
                    _cursorType = CursorType.Hand;
                }
            }
        }
    }
}
