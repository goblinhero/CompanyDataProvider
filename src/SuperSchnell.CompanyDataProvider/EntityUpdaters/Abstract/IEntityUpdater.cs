using System.Collections.Generic;
using NHibernate;

namespace SuperSchnell.CompanyDataProvider.EntityUpdaters.Abstract
{
    public interface IEntityUpdater<T>
    {
        long Id { get; }
        int Version { get; }
        bool TryUpdate(ISession session, ref T entity, out IEnumerable<string> errors);
    }
}