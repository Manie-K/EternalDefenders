using System;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "DebugStrategy", menuName = "EternalDefenders/Enemy/TargetStrategies/DebugStrategy")]
    public class DefaultEnemyTargetStrategy : EnemyTargetStrategy
    {
        readonly Collider[] _colliders = new Collider[64];
        
        public override IEnemyTarget FindTarget(EnemyController enemy)
        {
            TowerController tower = null;
            switch(priorityTarget)
            {
                case PriorityTarget.MainBase:
                    if(IsMainBaseValid(enemy))
                    {
                        return mainBase;
                    }

                    if(IsPlayerValid(enemy))
                    {
                        return player;
                    }
                    
                    tower = AreTowersValid(enemy);
                    if(tower != null)
                    {
                        return tower;
                    }

                    return mainBase;
                case PriorityTarget.Player:
                    if(IsPlayerValid(enemy))
                    {
                        return player;
                    }
                    if(IsMainBaseValid(enemy))
                    {
                        return mainBase;
                    }
                    tower = AreTowersValid(enemy);
                    if(tower != null)
                    {
                        return tower;
                    }

                    return mainBase;
                case PriorityTarget.Tower:
                    tower = AreTowersValid(enemy);
                    if(tower != null)
                    {
                        return tower;
                    }
                    if(IsMainBaseValid(enemy))
                    {
                        return mainBase;
                    }

                    if(IsPlayerValid(enemy))
                    {
                        return player;
                    }
                    return mainBase;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        bool IsMainBaseValid(EnemyController enemy) => Vector3.Distance(enemy.transform.position, mainBase.transform.position) <=
                                                       enemy.Stats.GetStat(StatType.SeekingRange);
        
        bool IsPlayerValid(EnemyController enemy) => Vector3.Distance(enemy.transform.position, player.transform.position) <=
                                                     enemy.Stats.GetStat(StatType.SeekingRange);
        
        TowerController AreTowersValid(EnemyController enemy)
        { 
            int foundCount = Physics.OverlapSphereNonAlloc(enemy.transform.position,
                enemy.Stats.GetStat(StatType.SeekingRange), _colliders);

            for(int i = 0; i < foundCount; i++)
            {
                TowerController tower = _colliders[i].gameObject.GetComponent<TowerController>();
                if(tower != null)
                {
                    return tower;
                }
            }
            return null;
        }
        
    }
}