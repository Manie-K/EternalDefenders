using System.Collections;
using UnityEngine;

namespace EternalDefenders
{
    public abstract class TowerAttackingStrategy : ScriptableObject
    {
        protected TowerController tower;

        public virtual void Init(TowerController tower)
        {
            this.tower = tower;
        }

        public abstract IEnumerator Attack(EnemyController target);
    }
}