using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    PlayerStat _stat;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _stat = GetComponent<PlayerStat>();
        _attackIcon = GameManager.Resource.Load<Texture2D>("Textures/Cursors/Attack");
        _handIcon = GameManager.Resource.Load<Texture2D>("Textures/Cursors/Hand");

        // 리스너 등록
        GameManager.Input.MouseAction -= OnMouseEvent;     // 두 번 등록 방지
        GameManager.Input.MouseAction += OnMouseEvent;
    }

    void Update()
    {
        switch (_state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
        }

        UpdateMouseCursor();
    }

    #region State
    Vector3 _destPos;
    Animator _animator;
    PlayerState _state = PlayerState.Idle;

    public enum PlayerState
    {
        Idle,
        Moving,
        Die,
        Skill,
    }
    void UpdateIdle()
    {
        _animator.SetFloat("speed", 0);
    }
    void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)    // float 오차 범위로
        {
            // 도착했을 때
            _state = PlayerState.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);

            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            nma.Move(dir.normalized * moveDist);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0) == false)
                    _state = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
        _animator.SetFloat("speed", _stat.MoveSpeed);
    }
    #endregion

    #region Mouse
    Texture2D _attackIcon;
    Texture2D _handIcon;
    CursorType _cursorType = CursorType.None;

    enum CursorType
    {
        None,
        Hand,
        Attack,
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

    GameObject _lockTarget;
    void OnMouseEvent(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die) return;

        // ScreenToWorldPoint() + direction.normalized
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Ground") | LayerMask.GetMask("Monster"));

        switch (evt)
        {
            case Define.MouseEvent.PointDown:
                if (raycastHit)
                {
                    // Raycast 충돌 발생하면 목적지로 지정
                    _destPos = hit.point;
                    _state = PlayerState.Moving;

                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
                        _lockTarget = hit.collider.gameObject;
                    else
                        _lockTarget = null;
                }
                break;
            case Define.MouseEvent.Press:
                if (_lockTarget != null)
                    _destPos = _lockTarget.transform.position;
                else if (raycastHit)
                    _destPos = hit.point;
                break;
            case Define.MouseEvent.PointUp:
                _lockTarget = null;
                break;
            case Define.MouseEvent.Click:
                break;
        }
    }
    #endregion
}
