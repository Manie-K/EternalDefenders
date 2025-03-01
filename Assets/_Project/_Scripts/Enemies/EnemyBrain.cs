using System;
using UnityEngine;

namespace EternalDefenders
{
    public class EnemyBrain : StateMachineBrain
    {
        EnemyController _enemyController;
        Coroutine _attackCoroutine;
        void OnDisable()
        {
            FSMEntitiesManager.Instance?.UnregisterEntity(this);
        }

        protected override void Start()
        {
            _enemyController = GetComponent<EnemyController>();
            base.Start();           
            
            navMeshAgent.speed = _enemyController.Stats.GetStat(StatType.Speed);
            FSMEntitiesManager.Instance.RegisterEntity(this);
            
            EnemyController.OnDeath += () => FSMEntitiesManager.Instance?.UnregisterEntity(this);
        }
        protected override void SetUpStateMachine()
        {
            var idleState = new EnemyIdleState(this);
            var walkState = new EnemyWalkState(this, _enemyController, navMeshAgent);
            var attackState = new EnemyAttackState(this);
            var deathState = new EnemyDeathState(this, navMeshAgent);
            
            //TODO set up more complex logic, set up Target and Attack strategies in EnemyController
            //we will call PickNewTarget from here somewhere
            
            stateMachine.AddTransition(walkState, attackState, new FuncPredicate(walkState.HasReachedDestination));
            stateMachine.AddTransition(attackState, idleState, new FuncPredicate(() =>
                _enemyController.Target.Stats.GetStat(StatType.Health) <= 0)
            );
            
            stateMachine.AddAnyTransition(deathState, new EventPredicate("OnDeath", _enemyController));
            
            stateMachine.SetState(walkState);
        }
        
        public void StartAttack(){_attackCoroutine = StartCoroutine(_enemyController.Attack());}
        public void StopAttack(){StopCoroutine(_attackCoroutine);}
    }
}