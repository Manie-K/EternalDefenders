using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "EternalDefenders/Stats/PlayerStats")]
    public class PlayerStats : StatsConfig
    {
        public int health;
        public int shield;
        public int damage;
        public int range;
        public int cooldown;
        public int speed;
        
        public override Dictionary<StatType, Stats.Stat> GetStats()
        {
            return new Dictionary<StatType, Stats.Stat>
            {
                { StatType.Health, new Stats.Stat(health) },
                { StatType.MaxHealth, new Stats.Stat(health) },
                { StatType.Shield, new Stats.Stat(shield) },
                { StatType.MaxShield, new Stats.Stat(shield) },
                { StatType.Damage, new Stats.Stat(damage) },
                { StatType.Range, new Stats.Stat(range) },
                { StatType.Speed, new Stats.Stat(speed) },
                { StatType.Cooldown, new Stats.Stat(cooldown) }
            };
        }
    }
}