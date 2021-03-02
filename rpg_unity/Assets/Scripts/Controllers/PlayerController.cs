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
        _stat = GetComponent<PlayerStat>();

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
            case PlayerState.Attack:
                UpdateAttack();
                break;
        }
    }

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
                    State = PlayerState.Moving;

                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
                        _lockTarget = hit.collider.gameObject;
                    else
                        _lockTarget = null;
                }
                break;
            case Define.MouseEvent.Press:
                if (_lockTarget != null)
                    _destPos = _lockTarget.transform.position;
                else if (_lockTarget == null && raycastHit)
                    _destPos = hit.point;
                break;
            case Define.MouseEvent.PointUp:
                _lockTarget = null;
                break;
            case Define.MouseEvent.Click:
                break;
        }
    }

    #region State
    Vector3 _destPos;
    GameObject _lockTarget;

    [SerializeField]
    PlayerState _state = PlayerState.Idle;
    public enum PlayerState
    {
        Idle,
        Moving,
        Die,
        Attack,
    }
    public PlayerState State
    {
        get { return _state; }
        set
        {
            _state = value;
            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case PlayerState.Die:
                    break;
                case PlayerState.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case PlayerState.Moving:
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case PlayerState.Attack:
                    anim.Play("ATTACK");
                    break;
            }
        }
    }
    void UpdateIdle()
    {
        State = PlayerState.Idle;
    }
    void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 1f)    // float 오차 범위로
        {
            // 도착했을 때
            if (_lockTarget != null)
                State = PlayerState.Attack;
            else
                State = PlayerState.Idle;
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
                    State = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    void UpdateAttack()
    {
        if (_lockTarget == null)
        {
            State = PlayerState.Idle;
            return;
        }
        Debug.Log($"Attack: {_lockTarget.name}");
        Vector3 dir = _lockTarget.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);

        State = PlayerState.Attack;
    }
    #endregion
}
