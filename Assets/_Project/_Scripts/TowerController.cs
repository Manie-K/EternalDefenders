using UnityEngine;
using UnityEngine.Serialization;

namespace EternalDefenders
{
    public class TowerController : MonoBehaviour
    {
        [SerializeField] TowerTargetingStrategy targetingStrategy;
        [SerializeField] TowerAttackingStrategy attackingStrategy;
        
        EnemyController _target;

        void Start()
        {
            targetingStrategy.Init(this);
            attackingStrategy.Init(this);
        }
    }
}
