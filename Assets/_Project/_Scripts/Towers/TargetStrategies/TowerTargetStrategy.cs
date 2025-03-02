using UnityEngine;

namespace EternalDefenders
{
    public abstract class TowerTargetStrategy : ScriptableObject
    {
        //<summary>
        //Validate if the target is still in range, etc.
        //If it is invalid, it returns null, if not, it doesn't change anything.
        //</summary>
        public abstract bool Validate(TowerController tower, EnemyController target);
        public abstract EnemyController FindTarget(TowerController tower);
    }
}