using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "DebugStrategy", menuName = "EternalDefenders/Enemy/TargetStrategies/DebugStrategy")]
    public class DebugEnemyTargetStrategy : EnemyTargetStrategy
    {
        public override TowerController FindTarget()
        {
            Debug.Log("Finding target");
            return FindFirstObjectByType<TowerController>();
        }
    }
}