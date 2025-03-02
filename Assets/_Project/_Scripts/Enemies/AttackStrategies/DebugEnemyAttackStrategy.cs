using System;
using UnityEngine;

namespace EternalDefenders
{    
    [CreateAssetMenu(fileName = "DebugStrategy", menuName = "EternalDefenders/Enemy/AttackStrategies/DebugStrategy")]
    public class DebugEnemyAttackStrategy : EnemyAttackStrategy
    {
        public override void Attack(EnemyController enemy, IEnemyTarget target)
        {
            Debug.Log("Attacking target");
            switch(target)
            {
                case MainBaseController mainBase:
                    DamageCalculator.EnemyAttackMainBase(enemy, mainBase);
                    break;
                case TowerController tower:
                    DamageCalculator.EnemyAttackTower(enemy, tower);
                    break;
                case PlayerController player:
                    DamageCalculator.EnemyAttackPlayer(enemy, player);
                    break;
                default:
                    Debug.LogError("Unknown target type");
                    break;
            }
        }

        public override bool TargetIsValid(EnemyController enemy, IEnemyTarget target)
        {
            if(target == null)
                return false;
            
            float distance = Vector3.Distance(((MonoBehaviour)target).transform.position, enemy.transform.position);
            return  distance <= enemy.Stats.GetStat(StatType.Range);
        }
    }
}