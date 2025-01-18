using System;
using UnityEngine;

namespace EternalDefenders
{
    public class TowerController : MonoBehaviour
    {
        [SerializeField] TowerStatsSO stats;
        [SerializeField] TowerTargetingStrategy targetingStrategy;
        [SerializeField] TowerAttackingStrategy attackingStrategy;
        
        public Stats Stats { get; private set; }
        
        EnemyController _target;
        CountdownTimer _cooldownTimer;
        
        void Start()
        {
            Stats = new Stats(stats.GetStats());
            targetingStrategy.Init(this);
            attackingStrategy.Init(this);
        }

        void Update()
        {
            if(_cooldownTimer.IsRunning) return;
            
            targetingStrategy.Validate(_target);

            if (_target == null)
            {
                targetingStrategy.FindTarget();
            }
            if (_target != null)
            {
                _cooldownTimer.StartTimer(Stats.GetStat(StatType.Cooldown));
                StartCoroutine(attackingStrategy.Attack(_target));
            }
        }
    }
}
