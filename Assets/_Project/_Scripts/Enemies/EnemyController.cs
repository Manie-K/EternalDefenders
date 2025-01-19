using System;
using UnityEngine;

namespace EternalDefenders
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] EnemyStats statsConfig;
        [SerializeField] Effect attackEffect;
        //[SerializeField] EnemyTargetStrategy targetStrategy;
        //[SerializeField] EnemyAttackStrategy attackStrategy;
        public Stats Stats { get; private set; }
        public Effect Effect { get; private set; }
        
        CountdownTimer _cooldownTimer;
        
        void Start()
        {
            Stats = new Stats(statsConfig.GetStats());
            Effect = attackEffect;
            
            //targetStrategy.Init(this);
            //attackStrategy.Init(this);
        }

        void Update()
        {
            Stats.UpdateStatsModifiers(Time.deltaTime);
            
        }
    }
}