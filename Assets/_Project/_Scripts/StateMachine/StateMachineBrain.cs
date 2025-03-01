using UnityEngine;
using UnityEngine.AI;

namespace EternalDefenders
{
    public abstract class StateMachineBrain : MonoBehaviour
    {
        protected StateMachine stateMachine = new();
        protected NavMeshAgent navMeshAgent;

        public IState CurrentState => stateMachine.CurrentState;
        protected virtual void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            
            if (navMeshAgent == null)
            {
                Debug.LogError("Missing navMeshAgent component");
            }
            
            SetUpStateMachine();
        }
        
        protected abstract void SetUpStateMachine();
        
        public virtual void OnUpdate()
        {
            stateMachine.OnUpdate();
        }

        public virtual void OnFixedUpdate()
        {
            stateMachine.OnFixedUpdate();
        }
        
    }
}