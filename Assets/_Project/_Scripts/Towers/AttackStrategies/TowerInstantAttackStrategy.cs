using System.Collections;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "InstantAttack", menuName = "EternalDefenders/Tower/AttackStrategies/InstantAttack")]
    public class TowerInstantAttackStrategy : TowerAttackStrategy
    {
        public override void Attack(TowerController tower, EnemyController target)
        {
            Debug.Log("Instant attack!");
            DamageCalculator.Instance.TowerAttackEnemy(tower, target);
        }
    }
}