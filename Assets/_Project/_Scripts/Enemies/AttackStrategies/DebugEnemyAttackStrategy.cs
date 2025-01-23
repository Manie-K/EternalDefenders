using UnityEngine;

namespace EternalDefenders
{    
    [CreateAssetMenu(fileName = "DebugStrategy", menuName = "EternalDefenders/Enemy/AttackStrategies/DebugStrategy")]
    public class DebugEnemyAttackStrategy : EnemyAttackStrategy
    {
        public override void Attack(IEnemyTarget target)
        {
            Debug.Log("Attacking target");
            DamageCalculator.PerformAttack(enemy, (TowerController)target);
        }

        public override bool ValidateTarget(IEnemyTarget target)
        {
            return target != null && Vector3.Distance(((MonoBehaviour)target).transform.position, enemy.transform.position)
                   <= enemy.Stats.GetStat(StatType.Range);
        }
    }
}