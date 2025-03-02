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

        public event Action OnDeath;
        
        CountdownTimer _cooldownTimer;

        void Awake()
        {
            targetStrategy.Init(this);
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

        //Don't know when to call this method xd
        public void PickNewTarget()
        {
            Target = targetStrategy.FindTarget();
        }
        
        public IEnumerator Attack()
        {
            while(attackStrategy.TargetIsValid(this, Target))
            {
                attackStrategy.Attack(this, Target);
                yield return new WaitForSeconds(Stats.GetStat(StatType.Cooldown));
            }
        }
        public void Die()
        {
            Debug.Log("I'm dead");
            
            OnDeath?.Invoke();
            GameStatisticsManager.Instance?.NotifyEnemyKilled();
            FSMEntitiesManager.Instance?.UnregisterEntity(GetComponent<EnemyBrain>());
            Destroy(gameObject);
        }
    }
}