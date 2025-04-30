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
        
        //Update target position every X frames
        const int UpdateTargetPositionFrameInterval = 60;
        int _frameCounter;
        
        public EnemyWalkState(EnemyBrain brain, EnemyController enemy, NavMeshAgent navMeshAgent)
            : base("EnemyWalk", brain)
        {
            _navMeshAgent = navMeshAgent;
            _enemyController = enemy;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _frameCounter = 0;
            ChangeAnimation(_walkHash);
            SetDestination();
        }

        
        public override void OnUpdate()
        {
            base.OnUpdate();
            _navMeshAgent.speed = _enemyController.Stats.GetStat(StatType.Speed);
            
            _frameCounter++;
            if(_frameCounter >= UpdateTargetPositionFrameInterval)
            {
                SetDestination();
                _frameCounter = 0;
            }
            
            if(_navMeshAgent.velocity.normalized != Vector3.zero)
                brain.transform.forward = _navMeshAgent.velocity.normalized;
        }

        public bool HasReachedDestination()
        {
            bool val = !_navMeshAgent.pathPending &&
                       _navMeshAgent.remainingDistance <= 0.01f &&
                       (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude <= 0.1f);
            if(val)
            {
                //Debug.Log("Reached destination");
            }
            return val;
        }

        void SetDestination()
        {
            if(brain == null || _enemyController.Target.Equals(null))
                return;
            
            Vector3 dir = (brain.transform.position - ((MonoBehaviour)_enemyController.Target).transform.position).normalized;
            Vector3 destination = ((MonoBehaviour)_enemyController.Target).transform.position + 
                                  (dir * (_enemyController.Stats.GetStat(StatType.Range) * 0.9f));
            _navMeshAgent.SetDestination(destination);
        }
    }
}