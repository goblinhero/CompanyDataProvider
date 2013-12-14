using System.Collections.Generic;
using SuperSchnell.CompanyDataProvider.Domain.Rules;

namespace SuperSchnell.CompanyDataProvider.Domain
{
    public abstract class Entity<T>:IEntity
        where T:class 
    {
        public virtual long? Id { get; protected set; }
        public virtual int Version { get; protected set; }
        public virtual bool IsValid(out IEnumerable<string> validationErrors)
        {
            var ruleset = new RuleSet<T>(GetValidationRules());
            return ruleset.UpholdsRules(this as T, out validationErrors);
        }

        protected virtual IEnumerable<IRule<T>> GetValidationRules()
        {
            return new IRule<T>[0];
        }
    }
}