using System;
using UnityEngine;

namespace EternalDefenders
{
    public abstract class EnemyBaseState : IState
    {
        public string Name { get; }
        public string Id { get; }
        protected readonly Animator animator;
        protected readonly EnemyBrain brain;
        protected const float CrossFadeDuration = 0.05f;
        
        protected EnemyBaseState(string name, EnemyBrain brain)
        {
            Name = name;
            Id = Guid.NewGuid().ToString();
            this.brain = brain;
            
            //TODO: Settle on hierarchy style
            animator = brain.transform.GetChild(0).GetComponent<Animator>();
            
            if(animator == null)
                throw new NullReferenceException("Animator is null");
        }
        
        public virtual void OnEnter()
        {
            //noop
        }

        public virtual void OnUpdate()
        {
            //noop
        }

        public virtual void OnFixedUpdate()
        {
            //noop
        }

        public virtual void OnExit()
        {
            //noop
        }
        
        protected void ChangeAnimation(int hash, float crossFadeDuration = CrossFadeDuration)
        {
            animator.StopPlayback();
            animator.Play(hash, 0, 0);
        }
    }
}