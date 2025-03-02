using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace EternalDefenders
{
    public abstract class TowerAttackStrategy : ScriptableObject
    { 
        public abstract void Attack(TowerController tower, EnemyController target);
    }
}