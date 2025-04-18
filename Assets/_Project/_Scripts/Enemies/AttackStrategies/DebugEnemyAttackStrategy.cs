﻿using System;
using UnityEngine;

namespace EternalDefenders
{    
    [CreateAssetMenu(fileName = "DebugStrategy", menuName = "EternalDefenders/Enemy/AttackStrategies/DebugStrategy")]
    public class DebugEnemyAttackStrategy : EnemyAttackStrategy
    {
        public override void Attack(EnemyController enemy, IEnemyTarget target)
        {
            if(target == null) 
                return;
            //Debug.Log("Attacking target");
            switch(target)
            {
                case MainBaseController mainBase:
                    DamageCalculator.Instance.EnemyAttackMainBase(enemy, mainBase);
                    break;
                case TowerController tower:
                    DamageCalculator.Instance.EnemyAttackTower(enemy, tower);
                    break;
                case PlayerController player:
                    DamageCalculator.Instance.EnemyAttackPlayer(enemy, player);
                    break;
                default:
                    Debug.LogError("Unknown target type");
                    break;
            }
        }

        public override bool TargetIsValid(EnemyController enemy, IEnemyTarget target)
        {
            if(target == null || target.Equals(null)) //Unity marks it as destroyed in C++ under the hood, but in C# there is ref missing
                return false;
            
            float distance = Vector3.Distance(((MonoBehaviour)target).transform.position, enemy.transform.position);
            return  distance <= enemy.Stats.GetStat(StatType.Range);
        }
    }
}