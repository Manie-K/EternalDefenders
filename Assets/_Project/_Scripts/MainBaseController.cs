using System;
using HudElements;
using MG_Utilities;
using UnityEngine;
using UnityEngine.UIElements;

namespace EternalDefenders
{
    public class MainBaseController : Singleton<MainBaseController>, IEnemyTarget
    {
        [SerializeField] MainBaseStats statsConfig;

        public event Action OnMainBaseDestroyed;
        public Stats Stats
        {
            get => _stats;
            set => _stats = value;
        }

        Stats _stats;

        void Start()
        {
            _stats = new Stats(statsConfig.GetStats());
        }

        void Update()
        {
            if (_stats.GetStat(StatType.Health) <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            OnMainBaseDestroyed?.Invoke();
        }
    }
}