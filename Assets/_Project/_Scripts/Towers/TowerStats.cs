using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "TowerStats", menuName = "EternalDefenders/Stats/TowerStats")]
    public class TowerStats : StatsConfig
    {
        public int health;
        public int damage;
        public int range;
        public int cooldown;
        
        public override Dictionary<StatType, Stats.Stat> GetStats()
        {
            return new Dictionary<StatType, Stats.Stat>
            {
                { StatType.Health, new Stats.Stat(health) },
                { StatType.MaxHealth, new Stats.Stat(health) },
                { StatType.Damage, new Stats.Stat(damage) },
                { StatType.Range, new Stats.Stat(range) },
                { StatType.Cooldown, new Stats.Stat(cooldown) }
            };
        }
    }
}