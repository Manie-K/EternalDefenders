using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "RayAttack", menuName = "EternalDefenders/Tower/AttackStrategies/RayAttack")]
    public class TowerRayAttackStrategy : TowerAttackStrategy
    {
        public override void Attack(TowerController tower, EnemyController target)
        {
            Debug.Log("Ray attack!");
            Debug.LogError("Not yet implemented!");
        }
    }
}