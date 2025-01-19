using System;

namespace EternalDefenders
{
    public class FuncPredicate : IPredicate
    {
        readonly Func<bool> _predicate;
        readonly Action _onTrueAction;
        
        public FuncPredicate(Func<bool> predicate, Action onTrueAction = null)
        {
            _predicate = predicate;
            _onTrueAction = onTrueAction;
        }

        public bool Evaluate()
        {
            bool result = _predicate.Invoke();
            if (result && _onTrueAction != null)
            {
                _onTrueAction.Invoke();
            }
            return result;
        }
    }
}