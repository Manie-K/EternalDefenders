using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "SphereStrategy", menuName = "EternalDefenders/Tower/TargetStrategies/SphereStrategy")]
    public class TowerSphereTargetStrategy : TowerTargetStrategy
    {
        readonly Collider[] _results = new Collider[32];
        public override bool Validate(EnemyController target)
        {
            if(target == null) return false;
            bool targetIsValid = true;
            
            if(Vector3.Distance(tower.transform.position, target.transform.position) > tower.Stats.GetStat(StatType.Range)) 
                targetIsValid = false;

            return targetIsValid;
        }

        public override EnemyController FindTarget()
        {
            int foundEnemies = Physics.OverlapSphereNonAlloc(tower.transform.position, tower.Stats.GetStat(StatType.Range), 
                _results);

            for (int i = 0; i < foundEnemies; i++)
            {
                EnemyController enemyController = _results[i].gameObject.GetComponent<EnemyController>();
                if (enemyController)
                {
                    return enemyController;
                }
            }

            return null;
        }
    }
}