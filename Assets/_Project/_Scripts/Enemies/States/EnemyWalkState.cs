using MG_Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace EternalDefenders
{
    public class EnemyWalkState : EnemyBaseState
    {
        readonly NavMeshAgent _navMeshAgent;
        readonly EnemyController _enemyController;
        readonly int _walkHash = Animator.StringToHash("Walk");

        public EnemyWalkState(EnemyBrain brain, EnemyController enemy, NavMeshAgent navMeshAgent)
            : base("EnemyWalk", brain)
        {
            _navMeshAgent = navMeshAgent;
            _enemyController = enemy;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            animator.CrossFade(_walkHash, CrossFadeDuration);
            Vector3 destination = ((MonoBehaviour)_enemyController.Target).transform.position + Random.insideUnitSphere.With(y:0) * 2;
            _navMeshAgent.SetDestination(destination);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if(_navMeshAgent.velocity.normalized != Vector3.zero)
                brain.transform.forward = _navMeshAgent.velocity.normalized;
        }

        public bool HasReachedDestination() => !_navMeshAgent.pathPending &&
                                               _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance &&
                                               (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0.0f);
    }
}