using UnityEngine;

namespace EternalDefenders
{
    public class EnemyIdleState : EnemyBaseState
    {
        readonly int _idleHash = Animator.StringToHash("Idle");
        public EnemyIdleState(EnemyBrain brain) : base("EnemyIdle", brain) { }

        public override void OnEnter()
        {
            base.OnEnter();
            animator.CrossFade(_idleHash, CrossFadeDuration);
        }
    }
}