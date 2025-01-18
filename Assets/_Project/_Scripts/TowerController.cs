using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace EternalDefenders
{
    public class TowerController : MonoBehaviour, IDamageable
    {
        [SerializeField] TowerStatsSO statsConfig;
        [SerializeField] TowerTargetingStrategy targetingStrategy;
        [SerializeField] TowerAttackingStrategy attackingStrategy;
        
        public Stats Stats { get; private set; }
        public EffectSO Effect => attackingStrategy.Effect;
        
        EnemyController _target;
        CountdownTimer _cooldownTimer;
        
        void Start()
        {
            Stats = new Stats(statsConfig.GetStats());
            targetingStrategy.Init(this);
            attackingStrategy.Init(this);
            _cooldownTimer = new CountdownTimer(Stats.GetStat(StatType.Cooldown));
        }

        void Update()
        {
            Stats.UpdateStatsModifiers(Time.deltaTime);
            if(_cooldownTimer.IsRunning) return;
            
            targetingStrategy.Validate(_target);

            if (_target == null)
            {
                targetingStrategy.FindTarget();
            }
            if (_target != null)
            {
                _cooldownTimer.Start(Stats.GetStat(StatType.Cooldown));
                StartCoroutine(attackingStrategy.Attack(_target));
            }
        }
    }
}
