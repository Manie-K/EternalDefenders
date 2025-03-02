using UnityEngine;

namespace EternalDefenders
{
    public abstract class EnemyAttackStrategy : ScriptableObject
    {
        public abstract void Attack(EnemyController enemy, IEnemyTarget target);
        public abstract bool TargetIsValid(EnemyController enemy, IEnemyTarget target);
    }
}