using System.Collections;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "ProjectileAttack", menuName = "EternalDefenders/Tower/AttackingStrategies/ProjectileAttack")]
    public class TowerProjectileAttackStrategy : TowerAttackingStrategy
    {
        public override IEnumerator Attack(EnemyController target)
        {
            throw new System.NotImplementedException();
        }
    }
}