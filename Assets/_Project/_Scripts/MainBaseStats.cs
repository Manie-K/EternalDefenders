using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "MainBaseStats", menuName = "EternalDefenders/Stats/MainBaseStats")]
    public class MainBaseStats : StatsConfig
    {
        public int health;

        public override Dictionary<StatType, Stats.Stat> GetStats()
        {
            return new Dictionary<StatType, Stats.Stat>
            {
                { StatType.Health, new Stats.Stat(health) },
                { StatType.MaxHealth, new Stats.Stat(health) },
            };
        }
    }
}