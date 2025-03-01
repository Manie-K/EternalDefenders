using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    public abstract class StatsConfig : ScriptableObject
    {
        public abstract Dictionary<StatType, Stats.Stat> GetStats();
    }
}