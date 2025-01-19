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
        [SerializeField] Transform attackPoint;
        public Stats Stats { get; private set; }
        public Effect Effect { get; private set; }
        public Transform AttackPoint => attackPoint;
        
        EnemyController _target;
        CountdownTimer _cooldownTimer;
        
        void Start()
        {
            Stats = new Stats(statsConfig.GetStats());
            Effect = attackEffect;
            
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
                targetStrategy.FindTarget();
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
