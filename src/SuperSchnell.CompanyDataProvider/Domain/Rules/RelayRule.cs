using System;

namespace SuperSchnell.CompanyDataProvider.Domain.Rules
{
    public class RelayRule<T> : IRule<T>
    {
        private Predicate<T> _isBroken;

        public RelayRule(Predicate<T> isBroken, string message)
        {
            _isBroken = isBroken;
            Message = message;
        }

        public bool IsBroken(T obj)
        {
            return _isBroken(obj);
        }

        public string Message { get; private set; }
    }
}