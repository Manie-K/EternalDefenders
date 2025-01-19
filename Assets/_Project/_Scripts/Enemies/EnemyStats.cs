using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "EternalDefenders/Stats/EnemyStats")]
    public class EnemyStats : StatsConfig
    {
        public int health;
        public int damage;
        public int range;
        public int shield;
        public int cooldown;
        public int returnDamage;
        
        public override Dictionary<StatType, Stats.Stat> GetStats()
        {
            return new Dictionary<StatType, Stats.Stat>
            {
                { StatType.Health, new Stats.Stat(health) },
                { StatType.MaxHealth, new Stats.Stat(health) },
                { StatType.Damage, new Stats.Stat(damage) },
                { StatType.Range, new Stats.Stat(range) },
                { StatType.Shield, new Stats.Stat(shield) },
                { StatType.Cooldown, new Stats.Stat(cooldown) },
                { StatType.ReturnDamage, new Stats.Stat(returnDamage) }
            };
        }
    }
}