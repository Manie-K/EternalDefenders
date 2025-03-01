using UnityEngine;

namespace EternalDefenders
{
    public abstract class EnemyAttackStrategy : ScriptableObject
    {
        protected EnemyController enemy;
        public virtual void Init(EnemyController controller)
        {
            enemy = controller;
        }
        public abstract void Attack(IEnemyTarget target);
        public abstract bool TargetIsValid(IEnemyTarget target);
    }
}