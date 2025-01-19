using UnityEngine;
using UnityEngine.Serialization;

namespace EternalDefenders
{
    public class TowerController : MonoBehaviour
    {
        [SerializeField] TowerStats statsConfig;
        [SerializeField] Effect attackEffect;
        [SerializeField] TowerTargetStrategy targetStrategy;
        [SerializeField] TowerAttackStrategy attackStrategy;
        public Stats Stats { get; private set; }
        public Effect Effect { get; private set; }
        public Transform AttackPoint => _attackPoint;
        
        EnemyController _target;
        CountdownTimer _cooldownTimer;
        Transform _attackPoint;
        
        void Start()
        {
            Stats = new Stats(statsConfig.GetStats());
            Effect = attackEffect;
            
            //TODO: This is a hardcoded value, we should find a way to get the attack point dynamically.
            _attackPoint = transform.GetChild(1);
            
            targetStrategy.Init(this);
            attackStrategy.Init(this);
            _cooldownTimer = new CountdownTimer(Stats.GetStat(StatType.Cooldown));
        }

        void Update()
        {
            Stats.UpdateStatsModifiers(Time.deltaTime);
            
            //==================
            //Could this part be a coroutine? With a WaitForSeconds(cooldown)? We could check the performance if needed.
            _cooldownTimer.Tick(Time.deltaTime);
            
            if(_cooldownTimer.IsRunning) return;
            
            if(!targetStrategy.Validate(_target))
            {
                _target = targetStrategy.FindTarget();
            }
            if(_target != null)
            {
                _cooldownTimer.Start(Stats.GetStat(StatType.Cooldown));
                attackStrategy.Attack(_target);
            }
            //==================
        }
    }
}
