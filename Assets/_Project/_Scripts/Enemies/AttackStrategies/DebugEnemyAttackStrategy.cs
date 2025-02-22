using UnityEngine;

namespace EternalDefenders
{    
    [CreateAssetMenu(fileName = "DebugStrategy", menuName = "EternalDefenders/Enemy/AttackStrategies/DebugStrategy")]
    public class DebugEnemyAttackStrategy : EnemyAttackStrategy
    {
        public override void Attack(IEnemyTarget target)
        {
            Debug.Log("Attacking target");
            MainBaseController mainBase = target as MainBaseController;
            TowerController tower = target as TowerController;
            if(mainBase != null)
            {
                DamageCalculator.PerformAttack(enemy, mainBase);
            }
            else if(tower != null)
            {
                DamageCalculator.PerformAttack(enemy, tower);
            }
        }

        public override bool TargetIsValid(IEnemyTarget target)
        {
            if(target == null)
                return false;
            
            var distance = Vector3.Distance(((MonoBehaviour)target).transform.position, enemy.transform.position);
            return  distance <= enemy.Stats.GetStat(StatType.Range);
        }
    }
}