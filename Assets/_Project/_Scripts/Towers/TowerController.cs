using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace EternalDefenders
{
    public class TowerController : MonoBehaviour, IEnemyTarget
    {
        [SerializeField] TowerStats statsConfig;
        [SerializeField] Effect attackEffect;
        [SerializeField] TowerTargetStrategy targetStrategy;
        [SerializeField] TowerAttackStrategy attackStrategy;
        
        public static event Action<TowerController> OnTowerDestroyed;

        public Stats Stats
        {
            get => _stats;
            set => _stats = value;
        }
        public Effect Effect { get; private set; }
        public Transform AttackPoint => _attackPoint;
        
        EnemyController _target;
        CountdownTimer _cooldownTimer;
        Transform _attackPoint;
        Stats _stats;
        
        void Start()
        {
            _stats = new Stats(statsConfig.GetStats());
            Effect = attackEffect;
            
            //TODO: This is a hardcoded value, we should find a way to get the attack point dynamically.
            _attackPoint = transform.GetChild(1);
            
            _cooldownTimer = new CountdownTimer(_stats.GetStat(StatType.Cooldown));
        }

        void Update()
        {
            _stats.UpdateStatsModifiers(Time.deltaTime);
            if(_stats.GetStat(StatType.Health) <= 0)
            {
                Die();
                return;
            }
            
            //==================
            //Could this part be a coroutine? With a WaitForSeconds(cooldown)? We could check the performance if needed.
            _cooldownTimer.Tick(Time.deltaTime);
            
            if(_cooldownTimer.IsRunning) return;
            
            if(!targetStrategy.Validate(this, _target))
            {
                _target = targetStrategy.FindTarget(this);
            }
            if(_target != null)
            {
                _cooldownTimer.Start(_stats.GetStat(StatType.Cooldown));
                attackStrategy.Attack(this, _target);
            }
            //==================
        }
        
        //TODO decide what to do in here
        void Die()
        {
            if (ItemManager.Instance.IsTowerProtected(this))
            {
                return;
            }

            OnTowerDestroyed?.Invoke(this);
            Debug.Log("Tower destroyed");
            Destroy(gameObject);
        }
    }
}
