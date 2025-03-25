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
        float _lastAttackTime;
        
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
                int cooldown = Stats.GetStat(StatType.Cooldown);
                if(Time.time - _lastAttackTime < cooldown)
                {
                    Debug.Log($"Prevented attack for {Time.time - _lastAttackTime}");
                    yield return new WaitForSeconds(cooldown - (Time.time - _lastAttackTime));
                }
                
                if(gameObject.GetComponent<EnemyBrain>().CurrentState.GetType() != typeof(EnemyAttackState))
                {
                    Debug.LogError($"Attacking while in different state ({gameObject.GetComponent<EnemyBrain>().CurrentState.Name})!");
                    yield break;
                }
                
                attackStrategy.Attack(this, Target);
                _lastAttackTime = Time.time;
                
                yield return new WaitForSeconds(cooldown);
            }
            OnRetarget?.Invoke();
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