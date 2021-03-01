using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    PlayerStat _stat;
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

    void Start()
    {
        _animator = GetComponent<Animator>();
        _stat = GetComponent<PlayerStat>();

        // 리스너 등록
        GameManager.Input.MouseAction -= OnMousePressed;     // 두 번 등록 방지
        GameManager.Input.MouseAction += OnMousePressed;
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
                _state = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
        _animator.SetFloat("speed", _stat.MoveSpeed);
    }
    void OnMousePressed(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die) return;
        if (evt != Define.MouseEvent.Press) return;

        // ScreenToWorldPoint() + direction.normalized
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ray를 최대 100f 거리만큼 발사하여 충돌체 정보를 hit에 저장
        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Ground") | LayerMask.GetMask("Monster")))
        {
            // Raycast 충돌 발생하면 목적지로 지정
            _destPos = hit.point;
            // Debug.Log($"destPost={_destPos}");
            _state = PlayerState.Moving;
        }

        // 메인 카메라 위치에서 dir 방향으로 100의 길이만큼 1초 동안 빨간색 광선 발사
        // Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);
    }
}
