using HudElements;
using MG_Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace EternalDefenders
{
    public class MainBaseController : Singleton<MainBaseController>, IEnemyTarget
    {

        public UIDocument hud;
        private HealthBar BaseHeartBar;

        public Stats Stats { get; set; }

        void Start()
        {
            var root = hud.rootVisualElement;
            BaseHeartBar = root.Q<HealthBar>("BaseHeartBar");

            var initialStats = new Dictionary<StatType, Stats.Stat>
            {
                { StatType.Health, new Stats.Stat(100) },
                { StatType.MaxHealth, new Stats.Stat(100) }
            };

            Stats = new Stats(initialStats);

            if (BaseHeartBar != null && Stats.HasStat(StatType.Health))
            {
                int currentHealth = Stats.GetStat(StatType.Health);
                int baseHealth = Stats.GetStat(StatType.MaxHealth);
                BaseHeartBar.value = (float)currentHealth / baseHealth;
            }
        }

        void Update()
        {
            if (BaseHeartBar != null) BaseHeartBar.value = (float) Stats.GetStat(StatType.Health) / Stats.GetStat(StatType.MaxHealth);
        }

    }
}
