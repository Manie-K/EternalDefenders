using System.Collections;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "RayAttack", menuName = "EternalDefenders/Tower/AttackingStrategies/RayAttack")]
    public class TowerRayAttackStrategy : TowerAttackingStrategy
    {
        public override IEnumerator Attack(EnemyController target)
        {
            throw new System.NotImplementedException();
        }
    }
}