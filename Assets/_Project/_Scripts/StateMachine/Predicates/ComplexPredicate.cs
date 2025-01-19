using System.Linq;

namespace EternalDefenders
{
    public class ComplexPredicate : IPredicate
    {
        readonly IPredicate[] _predicates;

        public ComplexPredicate(IPredicate[] predicates)
        {
            _predicates = predicates;
        }

        public bool Evaluate()
        {
            return _predicates.All(predicate => predicate.Evaluate());
        }
    }
}