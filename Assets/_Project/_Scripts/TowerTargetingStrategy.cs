using UnityEngine;

namespace EternalDefenders
{
    public abstract class TowerTargetingStrategy : ScriptableObject
    {
        protected TowerController tower;

        public virtual void Init(TowerController tower)
        {
            this.tower = tower;
        }

        public abstract void Validate();
        public abstract void FindTarget();
    }
}