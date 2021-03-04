using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField] int _exp;
    [SerializeField] int _gold;

    public int Exp
    {
        get { return _exp; }
        set
        {
            _exp = value;

            // Level-up check
            int level = Level;
            while (true)
            {
                Data.Stat stat;
                // 다음 레벨 없음
                if (Manager.Data.StatDict.TryGetValue(level + 1, out stat) == false)
                    break;
                // 현재 경험치가 다음 레벨 최고 경험치보다 적을 경우
                if (_exp < stat.totalExp)
                    break;
                level++;
            }

            if (level > Level)
            {
                Debug.Log("Level Up!");
                Level = level;
                SetStat(Level);
            }
        }
    }
    public int Gold { get { return _gold; } set { _gold = value; } }

    void Start()
    {
        _level = 1;
        _moveSpeed = 10.0f;
        _gold = 0;
        _exp = 0;

        SetStat(_level);
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Manager.Data.StatDict;
        Data.Stat stat = dict[_level];
        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
        _defence = stat.defence;
    }

    protected override void OnDead(Stat attacker)
    {
        Debug.Log("Player Dead");
    }
}
