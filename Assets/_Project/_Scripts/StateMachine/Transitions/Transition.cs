namespace EternalDefenders
{
    public class Transition : ITransition
    {
        public IState To { get; }
        public IPredicate Condition { get; }

        public Transition(IState targetState, IPredicate condition)
        {
            To = targetState;
            Condition = condition;
        }
    }
}