using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    PlayerStat _stat;

    public override void init()
    {
        _stat = GetComponent<PlayerStat>();

        // 리스너 등록
        Manager.Input.MouseAction -= OnMouseEvent;     // 두 번 등록 방지
        Manager.Input.MouseAction += OnMouseEvent;

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Manager.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    void OnMouseEvent(Define.MouseEvent evt)
    {
        if (_state == Define.State.Die) return;

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
                    State = Define.State.Moving;

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
    void OnHitEvent()
    {
        if (_lockTarget == null)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Attack;

            // 체력 감소시키기
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            int damage = Mathf.Max(0, _stat.Attack - targetStat.Defence);
            targetStat.Hp -= damage;
        }
    }

    #region State

    protected override void UpdateMoving()
    {
        // 타겟 오브젝트가 있을 경우
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;

            if (distance < 1.0f)
            {
                State = Define.State.Attack;
                return;
            }
        }

        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)    // float 오차 범위로
        {
            // 도착했을 때
            State = Define.State.Idle;
        }
        else
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0) == false)
                    State = Define.State.Idle;
                return;
            }

            // 이동
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }
    protected override void UpdateAttack()
    {
        base.UpdateAttack();

        if (_lockTarget != null)
        {
            // 공격할 때 바라보도록
            Vector3 dir = _lockTarget.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }
    #endregion
}