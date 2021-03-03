using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    Stat _stat;
    [SerializeField] float _scanRange = 10.0f;
    [SerializeField] float _attackRange = 2.0f;

    public override void init()
    {
        _stat = gameObject.GetComponent<Stat>();

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            GameManager.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }
    void OnHitEvent()
    {
        if (_lockTarget == null)
        {
            State = Define.State.Idle;
        }
        else
        {
            // 체력 감소시키기
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            int damage = Mathf.Max(0, _stat.Attack - targetStat.Defence);
            targetStat.Hp -= damage;

            if (targetStat.Hp > 0)
            {
                float distance = (_lockTarget.transform.position - transform.position).magnitude;

                if (distance < _attackRange)
                    State = Define.State.Attack;
                else
                    State = Define.State.Moving;
            }
            else
            {
                State = Define.State.Idle;
            }
        }
    }

    #region  State

    protected override void UpdateIdle()
    {
        base.UpdateIdle();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        float distance = (player.transform.position - transform.position).magnitude;
        if (distance < _scanRange)
        {
            _lockTarget = player;
            State = Define.State.Moving;
            return;
        }
    }

    protected override void UpdateMoving()
    {
        base.UpdateMoving();

        // 1. 타겟 오브젝트가 있을 경우
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;

            if (distance < _attackRange)
            {
                NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
                nma.SetDestination(transform.position);

                State = Define.State.Attack;
                return;
            }
        }

        // 2. 이동
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)    // float 오차 범위로
        {
            // 도착했을 때
            State = Define.State.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            nma.SetDestination(_destPos);
            nma.speed = _stat.MoveSpeed;

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
