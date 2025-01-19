using System.Collections;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "RayAttack", menuName = "EternalDefenders/Tower/AttackStrategies/RayAttack")]
    public class TowerRayAttackStrategy : TowerAttackStrategy
    {
        public override void Attack(EnemyController target)
        {
            Debug.Log("Ray attack!");
        }
    }
}