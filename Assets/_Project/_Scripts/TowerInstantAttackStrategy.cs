using System.Collections;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "InstantAttack", menuName = "EternalDefenders/Tower/AttackingStrategies/InstantAttack")]
    public class TowerInstantAttackStrategy : TowerAttackingStrategy
    {
        public override IEnumerator Attack(EnemyController target)
        {
            Debug.Log("Instant attack!"); //TODO implement attacks
            yield return null;
        }
    }
}