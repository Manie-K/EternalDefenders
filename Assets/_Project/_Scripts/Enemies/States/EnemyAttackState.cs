using UnityEngine;

namespace EternalDefenders
{
    public class EnemyAttackState : EnemyBaseState
    {
        readonly int _attackHash = Animator.StringToHash("Attack");

        public EnemyAttackState(EnemyBrain brain) : base("EnemyAttack", brain)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            ChangeAnimation(_attackHash);
            brain.StartAttack();
        }
        public override void OnExit()
        {
            base.OnExit();
            brain.StopAttack();
        }
    }
}