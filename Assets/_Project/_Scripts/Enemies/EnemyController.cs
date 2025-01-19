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
        public TowerController Target { get; private set; }

        public event Action OnDeath;
        
        CountdownTimer _cooldownTimer;

        void Awake()
        {
            PickNewTarget();
        }

        void Start()
        {
            Stats = new Stats(statsConfig.GetStats());
            Effect = attackEffect;
            
            targetStrategy.Init(this);
            attackStrategy.Init(this);
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
            while(attackStrategy.ValidateTarget(Target))
            {
                attackStrategy.Attack(Target);
                yield return new WaitForSeconds(Stats.GetStat(StatType.Cooldown));
            }
        }
        public void Die()
        {
            OnDeath?.Invoke();
            Debug.Log("I'm dead");
        }
    }
}