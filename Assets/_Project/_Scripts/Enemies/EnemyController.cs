using System;
using System.Collections;
using UnityEngine;

namespace EternalDefenders
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] EnemyStats statsConfig;
        [SerializeField] Effect attackEffect;
        [SerializeField] EnemyTargetStrategy targetStrategy;
        [SerializeField] EnemyAttackStrategy attackStrategy;
        public Stats Stats { get; private set; }
        public Effect Effect { get; private set; }
        public IEnemyTarget Target { get; private set; }

        public static event EventHandler OnDeath;
        
        CountdownTimer _cooldownTimer;

        void Awake()
        {
            targetStrategy.Init(this);
            attackStrategy.Init(this);
            Stats = new Stats(statsConfig.GetStats());
            Effect = attackEffect;
            
            PickNewTarget();
        }
        void Update()
        {
            Stats.UpdateStatsModifiers(Time.deltaTime);
            if(Stats.GetStat(StatType.Health) <= 0)
            {
                Die();
            }
        }

        //It should be called from EnemyBrain component.
        public void PickNewTarget()
        {
            Target = targetStrategy.FindTarget();
        }
        
        public IEnumerator Attack()
        {
            while(attackStrategy.TargetIsValid(Target))
            {
                attackStrategy.Attack(Target);
                yield return new WaitForSeconds(Stats.GetStat(StatType.Cooldown));
            }
        }
        public void Die()
        {
            OnDeath?.Invoke(this, EventArgs.Empty);
            Debug.Log("I'm dead");
        }
    }
}