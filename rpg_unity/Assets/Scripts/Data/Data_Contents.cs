using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    #region Stat

    [Serializable]
    public class Stat
    {
        // public으로 하거나 [SerializeField] 사용
        // 이름 동일해야 함.
        // type도 자동으로 parsring
        public int level;
        public int maxHp;
        public int attack;
        public int defence;
        public int totalExp;
    }

    [Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
            foreach (Stat stat in stats)
                dict.Add(stat.level, stat);

            return dict;
        }
    }

    #endregion
}