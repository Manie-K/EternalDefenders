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
        [SerializeField] float retargetingInterval = 2f;
        public Stats Stats { get; private set; }
        public Effect Effect { get; private set; }
        public IEnemyTarget Target { get; private set; }
        public event Action OnDeath;
        public event Action OnRetarget;

        CountdownTimer _retargetingTimer;
        
        void Awake()
        {
            targetStrategy.Init();
            Stats = new Stats(statsConfig.GetStats());
            Effect = attackEffect;
            
            _retargetingTimer = new CountdownTimer(retargetingInterval);
            _retargetingTimer.Start();
        }

        void Start()
        {
            PickNewTarget();
            _retargetingTimer.OnTimerStop += PickNewTarget;
            _retargetingTimer.OnTimerStop += () => _retargetingTimer.Start();
        }

        void OnDisable()
        {
            _retargetingTimer.OnTimerStop -= PickNewTarget;
        }

        void Update()
        {
            _retargetingTimer.Tick(Time.deltaTime);
            Stats.UpdateStatsModifiers(Time.deltaTime);
            if(Stats.GetStat(StatType.Health) <= 0)
            {
                Die();
            }
            else if(Stats.GetStat(StatType.Health) > 300)
            {
                Debug.LogError("Enemy gaining health instead of losing it");
            }
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
        void PickNewTarget()
        {
            Target = targetStrategy.FindTarget(this);
            OnRetarget?.Invoke();
            //Debug.Log("I've picked new target: " + Target);
        }

        void OnDrawGizmosSelected()
        {
            if(!Application.isPlaying) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Stats.GetStat(StatType.Range));
        }
    }
}