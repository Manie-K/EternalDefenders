using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace EternalDefenders
{
    public abstract class TowerAttackStrategy : ScriptableObject
    {
        protected TowerController tower;
        public virtual void Init(TowerController tower)
        {
            this.tower = tower;
        }

        public abstract void Attack(EnemyController target);
    }
}