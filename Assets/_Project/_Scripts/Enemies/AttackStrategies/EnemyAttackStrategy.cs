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
        public abstract void Attack(TowerController target);
        public abstract bool ValidateTarget(TowerController target);
    }
}