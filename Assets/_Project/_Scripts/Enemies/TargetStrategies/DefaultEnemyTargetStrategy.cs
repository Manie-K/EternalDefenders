using System;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "DebugStrategy", menuName = "EternalDefenders/Enemy/TargetStrategies/DebugStrategy")]
    public class DefaultEnemyTargetStrategy : EnemyTargetStrategy
    {
        readonly Collider[] _colliders = new Collider[64];
        
        public override IEnemyTarget FindTarget()
        {
            Debug.Log("Finding target");
            TowerController tower = null;
            switch(priorityTarget)
            {
                case PriorityTarget.MainBase:
                    if(IsMainBaseValid())
                    {
                        return mainBase;
                    }

                    if(IsPlayerValid())
                    {
                        return player;
                    }
                    
                    tower = AreTowersValid();
                    if(tower != null)
                    {
                        return tower;
                    }

                    return mainBase;
                case PriorityTarget.Player:
                    if(IsPlayerValid())
                    {
                        return player;
                    }
                    if(IsMainBaseValid())
                    {
                        return mainBase;
                    }
                    tower = AreTowersValid();
                    if(tower != null)
                    {
                        return tower;
                    }

                    return mainBase;
                case PriorityTarget.Tower:
                    tower = AreTowersValid();
                    if(tower != null)
                    {
                        return tower;
                    }
                    if(IsMainBaseValid())
                    {
                        return mainBase;
                    }

                    if(IsPlayerValid())
                    {
                        return player;
                    }
                    return mainBase;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        bool IsMainBaseValid() => Vector3.Distance(enemy.transform.position, mainBase.transform.position) <=
                                  enemy.Stats.GetStat(StatType.SeekingRange);
        
        bool IsPlayerValid() => Vector3.Distance(enemy.transform.position, player.transform.position) <=
                               enemy.Stats.GetStat(StatType.SeekingRange);
        
        TowerController AreTowersValid()
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