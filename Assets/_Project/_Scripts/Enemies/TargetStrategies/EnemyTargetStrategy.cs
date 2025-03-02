using UnityEngine;

namespace EternalDefenders
{
    public abstract class EnemyTargetStrategy : ScriptableObject
    {
        protected enum PriorityTarget
        {
            MainBase,
            Player,
            Tower
        }
        
        [SerializeField] protected PriorityTarget priorityTarget;
        
        protected PlayerController player;
        protected MainBaseController mainBase;

        protected const string MainBaseTag = "MainBase";
        protected const string PlayerTag = "Player";
        

        public virtual void Init()
        {
            player = GameObject.FindGameObjectWithTag(PlayerTag).GetComponent<PlayerController>();
            mainBase = GameObject.FindGameObjectWithTag(MainBaseTag).GetComponent<MainBaseController>();
        }

        public abstract IEnemyTarget FindTarget(EnemyController enemy);
    }
}