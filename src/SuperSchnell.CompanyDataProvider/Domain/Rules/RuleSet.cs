using System.Collections.Generic;
using System.Linq;

namespace SuperSchnell.CompanyDataProvider.Domain.Rules
{
    public class RuleSet<T>
    {
        private readonly IEnumerable<IRule<T>> _rules;

        public RuleSet(IEnumerable<IRule<T>> rules)
        {
            _rules = rules;
        }
        public bool UpholdsRules(T obj, out IEnumerable<string> errors)
        {
            errors = _rules.Where(r => r.IsBroken(obj)).Select(r => r.Message).ToList();
            return !errors.Any();
        }
    }
}