using UnityEngine;

namespace EternalDefenders
{
    public abstract class EnemyTargetStrategy : ScriptableObject
    {
        protected EnemyController enemy;
        public virtual void Init(EnemyController controller)
        {
            enemy = controller;
        }
        public abstract TowerController FindTarget();
    }
}