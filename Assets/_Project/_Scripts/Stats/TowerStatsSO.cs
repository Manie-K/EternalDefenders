using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "TowerStatsSO", menuName = "EternalDefenders/Stats/TowerStatsSO")]
    public class TowerStatsSO : ScriptableObject
    {
        public int health;
        public int damage;
        public int range;
        public int cooldown;
        
        public Dictionary<StatType, int> GetStats()
        {
            return new Dictionary<StatType, int>
            {
                {StatType.Health, health},
                { StatType.Damage, damage},
                { StatType.Range, range},
                { StatType.Cooldown, cooldown}
            };
        }
    }
}