using System.Collections.Generic;
using NHibernate;

namespace SuperSchnell.CompanyDataProvider.EntityUpdaters.Abstract
{
    public abstract class BaseEntityUpdater<TEntity>:IEntityUpdater<TEntity>
    {
        protected BaseEntityUpdater(long id, int version)
        {
            Id = id;
            Version = version;
        }

        public long Id { get; private set; }
        public int Version { get; private set; }
        public bool TryUpdate(ISession session, ref TEntity entity, out IEnumerable<string> errors)
        {
            errors = new string[0];
            return true;
        }
    }
}
