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

        
        //<summary>
        //Validate if the target is still in range, etc.
        //If it is invalid, it returns null, if not, it doesn't change anything.
        //</summary>
        public abstract bool Validate(EnemyController target);
        public abstract EnemyController FindTarget();
    }
}