using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace EternalDefenders
{
    public abstract class TowerAttackingStrategy : ScriptableObject
    {
        public EffectSO Effect { get; } //need to be assigned in editor
        
        protected TowerController tower;
        public virtual void Init(TowerController tower)
        {
            this.tower = tower;
        }

        public abstract IEnumerator Attack(EnemyController target);
    }
}