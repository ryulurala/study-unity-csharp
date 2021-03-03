using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Vector3 _destPos;
    [SerializeField]
    protected GameObject _lockTarget;
    [SerializeField]
    protected Define.State _state = Define.State.Idle;

    public virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;
            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case Define.State.Die:
                    break;
                case Define.State.Idle:
                    anim.CrossFade("IDLE", 0.1f);
                    break;
                case Define.State.Moving:
                    anim.CrossFade("MOVE", 0.1f);
                    break;
                case Define.State.Attack:
                    anim.CrossFade("ATTACK", 0.1f);
                    break;
            }
        }
    }

    void Start()
    {
        init();
    }

    void Update()
    {
        switch (_state)
        {
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Attack:
                UpdateAttack();
                break;
        }
    }

    public abstract void init();
    virtual protected void UpdateDie() { }
    virtual protected void UpdateIdle() { }
    virtual protected void UpdateMoving() { }
    virtual protected void UpdateAttack() { }
}
