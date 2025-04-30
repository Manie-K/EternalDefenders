//The whole grand design of this FSM system is inspired by the video from git-amend, which can be found here: 
//https://www.youtube.com/watch?v=NnH6ZK5jt7o&t=122s&ab_channel=git-amend
using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    public class StateMachine
    {
        class StateNode
        {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new();
            }
            public void AddTransition(ITransition transition)
            {
                Transitions.Add(transition);
            }
        }
        
        public IState CurrentState => _currentStateNode.State;
        StateNode _currentStateNode;
        readonly Dictionary<string, StateNode> _stateNodes = new();
        readonly HashSet<ITransition> _anyTransitions = new(); //Transitions which are available from every other state
        
        
        StateNode GetOrAddStateNode(IState state)
        {
            StateNode foundStateNode = _stateNodes.GetValueOrDefault(state.Id);
            if (foundStateNode == null)
            {
                foundStateNode = new StateNode(state);
                _stateNodes.Add(state.Id, foundStateNode);
            }

            return foundStateNode;
        }

        public void AddAnyTransition(IState to, IPredicate condition)
        {
            GetOrAddStateNode(to);
            _anyTransitions.Add(new Transition(to, condition));
        }
        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetOrAddStateNode(to);
            GetOrAddStateNode(from).AddTransition(new Transition(to, condition));
        }
        
        ITransition GetAvailableTransition()
        {
            foreach(var transition in _anyTransitions)
            {
                if (transition.Condition.Evaluate())
                    return transition;
            }

            foreach (var transition in _currentStateNode.Transitions)
            {
                if (transition.Condition.Evaluate())
                    return transition;
            }

            return null;
        }
        
        void ChangeState(IState newState)
        {
            string currentStateName = _currentStateNode?.State?.Name;
            if (_currentStateNode?.State == newState)
                return;

            _currentStateNode?.State?.OnExit();
            SetState(newState);
            //Debug.Log($"Changed state from {currentStateName} to {newState.Name}");
        }
        public void SetState(IState newState)
        {
            _currentStateNode = _stateNodes[newState.Id];
            _currentStateNode.State?.OnEnter();
        }
        
        public void OnUpdate()
        {
            ITransition transition = GetAvailableTransition();
            if (transition != null)
                ChangeState(transition.To);
            else
                _currentStateNode?.State?.OnUpdate();
        }
        public void OnFixedUpdate()
        {
            _currentStateNode?.State?.OnFixedUpdate();
        }
    }
}