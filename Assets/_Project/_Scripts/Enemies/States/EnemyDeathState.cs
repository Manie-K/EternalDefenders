using UnityEngine;
using UnityEngine.AI;

namespace EternalDefenders
{
    public class EnemyDeathState : EnemyBaseState
    {
        readonly NavMeshAgent _navMeshAgent;
        readonly int _dieHash = Animator.StringToHash("Die");

        public EnemyDeathState(EnemyBrain brain, NavMeshAgent navMeshAgent) : base("EnemyDeath", brain)
        {
            _navMeshAgent = navMeshAgent;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _navMeshAgent.isStopped = true;
            ChangeAnimation(_dieHash);
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == _dieHash 
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                brain.gameObject.SetActive(false);
            }
        }
    }
}