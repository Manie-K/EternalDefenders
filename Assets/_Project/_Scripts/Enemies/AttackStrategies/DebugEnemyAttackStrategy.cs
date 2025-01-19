using UnityEngine;

namespace EternalDefenders
{    
    [CreateAssetMenu(fileName = "DebugStrategy", menuName = "EternalDefenders/Enemy/AttackStrategies/DebugStrategy")]
    public class DebugEnemyAttackStrategy : EnemyAttackStrategy
    {
        public override void Attack(TowerController target)
        {
            Debug.Log("Attacking target");
            DamageCalculator.PerformAttack(enemy, target);
        }

        public override bool ValidateTarget(TowerController target)
        {
            return target != null && Vector3.Distance(target.transform.position, enemy.transform.position)
                   <= enemy.Stats.GetStat(StatType.Range);
        }
    }
}